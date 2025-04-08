using UnityEngine;

namespace KenTank.GameManager
{
    public class SceneActions : MonoBehaviour
    {
        public static void LoadScene(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }

        public static void LoadSceneWithLoading(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex, true);
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static void LoadSceneWithLoading(string sceneName)
        {
            SceneManager.LoadScene(sceneName, true);
        }

        public static void RestartScene(bool loading = false)
        {
            SceneManager.RestartScene(loading);
        }

        public static void NextScene(bool loading = false)
        {
            SceneManager.NextScene(loading);
        }

        public static void BackScene(bool loading = false)
        {
            SceneManager.BackScene(loading);
        }
    }
}