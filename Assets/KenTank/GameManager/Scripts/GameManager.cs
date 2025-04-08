using DG.Tweening;
using UnityEngine;

namespace KenTank.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public SceneManager sceneManager {get; private set;}
        public AudioManager audioManager {get; private set;}
        public SettingsManager settingsManager {get; private set;}

        public void Init() 
        {
            // Collect Childern Component
            sceneManager = transform.GetComponentInChildren<SceneManager>();
            audioManager = transform.GetComponentInChildren<AudioManager>();
            settingsManager = transform.GetComponentInChildren<SettingsManager>();

            // Check instance
            if (instance)
            {
                instance.sceneManager.sceneTransition = sceneManager.sceneTransition;
                instance.sceneManager.sceneMusic = sceneManager.sceneMusic;
                instance.sceneManager.volume = sceneManager.volume;
                instance.sceneManager.loop = sceneManager.loop;
                sceneManager.onSceneEnter.Invoke();
                timeScale = 1;
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                settingsManager.Init();
                DontDestroyOnLoad(gameObject);
            }

            instance.sceneManager.PlayCurrentMusic();
        }

        void Awake()
        {
            Init();
        }

        public static float timeScale {
            get {
                return Time.timeScale;
            }
            set {
                Time.timeScale = value;
            }
        }

        public static void SetTimeScale(float value, float duration = 0)
        {
            DOTween.Kill(instance.gameObject);
            DOTween.To(x => timeScale = x, timeScale, value, duration).SetTarget(instance.gameObject);
        }

        public static void Quit() 
        {
            DOTween.KillAll();
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}