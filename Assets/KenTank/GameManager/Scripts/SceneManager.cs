using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using SceneM = UnityEngine.SceneManagement.SceneManager;

namespace KenTank.GameManager
{
    public class SceneManager : MonoBehaviour
    {
        public SceneTransition sceneTransition;
        public AudioClip sceneMusic;
        [Range(0, 1)] public float volume = 1;
        public bool loop = true;
        
        [Header("Events")]
        public UnityEvent onSceneEnter;

        public static Action onLoadStart;
        public static Action onLoadComplete;
        public static float progress = 0.0f;
        public static SceneTransition transition {get; private set;}
        public static AudioSource playingMusic {get; private set;}
        public static int previousSceneIndex;
        public static AsyncOperation loadingProcess = null;

        public void PlayCurrentMusic() 
        {
            if (sceneMusic)
            {
                if (!playingMusic)
                {
                    var item = AudioManager.instance.CreateMusicInstance(sceneMusic, volume, onComplete: () => {
                        playingMusic = null;
                        if (GameManager.instance.sceneManager.loop)
                        {
                            GameManager.instance.sceneManager.PlayCurrentMusic();
                        }
                    });
                    playingMusic = item;
                }
                else
                {
                    if (playingMusic.clip != sceneMusic)
                    {
                        playingMusic!.DOFade(0, 0.5f).SetTarget(playingMusic.gameObject)
                        .onComplete = () => {
                            DOTween.Kill(playingMusic.gameObject);
                            Destroy(playingMusic.gameObject);
                            playingMusic = null;
                            GameManager.instance.sceneManager.PlayCurrentMusic();
                        };
                    }
                    else
                    {
                        playingMusic!.DOFade(GameManager.instance.sceneManager.volume, 0.5f).SetTarget(playingMusic.gameObject);
                    }
                }
            }
        }

        public static void StopCurrentMusic() 
        {
            if (playingMusic)
            {
                DOTween.Kill(playingMusic.gameObject);
                var tmp = playingMusic;
                tmp.DOFade(0, 0.5f).SetTarget(playingMusic.gameObject)
                .onComplete = () => {
                    Destroy(tmp.gameObject);
                };
                playingMusic = null;
            }
        }

        public static void SetCurrentMusicVolume(float value)
        {
            if (playingMusic)
            {
                DOTween.Kill(playingMusic.gameObject);
                playingMusic.DOFade(value, 0.5f).SetTarget(playingMusic.gameObject);
            }
        }

        static float loadTime;
        public static async void LoadScene(int buildIndex, bool showLoading = false)
        {
            if (loadingProcess != null) return;

            onLoadStart?.Invoke();
            progress = 0;
            loadTime = 0;

            bool isTransitionComplete = true;
            var instanceTransation = GameManager.instance.sceneManager.sceneTransition;
            if (instanceTransation) 
            {
                isTransitionComplete = false;
                transition = Instantiate(instanceTransation);
                transition.onShowComplete += () => isTransitionComplete = true;
                transition.ShowLoading(showLoading);
                transition.Show();
                DontDestroyOnLoad(transition.gameObject);
            }
            previousSceneIndex = SceneM.GetActiveScene().buildIndex;
            var process = SceneM.LoadSceneAsync(buildIndex);
            loadingProcess = process;
            process.allowSceneActivation = false;
            
            while (process.progress < 0.9f || !isTransitionComplete)
            {
                loadTime += Time.unscaledDeltaTime;
                progress = process.progress;
                if (loadTime > 5 && !transition.isShowLoading)
                {
                    transition.ShowLoading(true);
                }
                await Task.Yield();
            }

            process.allowSceneActivation = true;

            while (!process.isDone) await Task.Yield();
            
            if (transition) 
            {
                transition.ShowLoading(false);
                transition.Hide();
                transition = null;
            }

            onLoadComplete?.Invoke();
            loadingProcess = null;
        }

        public static void LoadScene(int buildIndex) { LoadScene(buildIndex); }
        public static void LoadSceneWithLoading(int buildIndex) { LoadScene(buildIndex, true); }

        public static void LoadScene(string sceneName, bool showLoading = false)
        {
            int index = -1;
            for (int i = 0; i < SceneM.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                if (sceneNameFromPath == sceneName)
                {
                    index = i;
                }
            }
            LoadScene(index, showLoading);
        }

        public static void LoadScene(string sceneName) { LoadScene(sceneName, false); }
        public static void LoadSceneWithLoading(string sceneName) { LoadScene(sceneName, true); }

        public static void RestartScene(bool loading = false)
        {
            var scene = SceneM.GetActiveScene().buildIndex;
            LoadScene(scene, loading);
        }

        public static void NextScene(bool loading = false) 
        {
            var scene = SceneM.GetActiveScene().buildIndex + 1;
            LoadScene(scene, loading);
        }

        public static void BackScene(bool loading = false) 
        {
            var scene = SceneM.GetActiveScene().buildIndex - 1;
            if (scene < 0) scene = 0;
            LoadScene(scene, loading);
        }

        public static void PreviousScene(bool loading = false) 
        {
            if (previousSceneIndex == SceneM.GetActiveScene().buildIndex)
            {
                Debug.Log("Previous Scene was Null");
                return;
            }
            LoadScene(previousSceneIndex, loading);
        }
    }
}