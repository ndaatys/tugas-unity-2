using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Mini_Game.Scripts
{
    public class Manager : MonoBehaviour
    {
        public static Manager instance = null;

        [SerializeField] UnityEvent onStartGame;
        [SerializeField] UnityEvent ongameOver;

        public void StartGame()
        {
            onStartGame.Invoke();
            Time.timeScale = 1;
        }   

        public void GameOver() 
        {
            ongameOver.Invoke();
            Time.timeScale = 0;
        }
        
        public void Reset() 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void Awake()
        {
            instance = this;
            Time.timeScale = 0;
        }
    }
}