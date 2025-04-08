using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace KenTank.GameManager
{
    public class SceneTransition : MonoBehaviour
    {
        [SerializeField] CanvasGroup root;
        [SerializeField] float fadeDuration = 0.5f;
        [SerializeField] Slider progressBar;
        

        public bool isShowLoading {get;set;} = false;
        public Action onShowComplete;
        public Action onHideComplete;

        public void Show() 
        {
            gameObject.SetActive(true);
            root.alpha = 0;
            root.DOFade(1, fadeDuration)
            .SetUpdate(true)
            .onComplete = () => {
                onShowComplete?.Invoke();
            };
        }

        public void Hide()
        {
            root.DOFade(0, fadeDuration)
            .SetUpdate(true)
            .onComplete = () => {
                onHideComplete?.Invoke();
                Destroy(gameObject);
            };
        }

        public void ShowLoading(bool value)
        {
            isShowLoading = value;
            if (!progressBar) return;
            progressBar.gameObject.SetActive(value);
        }

        void Awake()
        {
            if (progressBar)
            {
                isShowLoading = progressBar.gameObject.activeSelf;
            }
        }

        void Update()
        {
            if (progressBar)
            {
                progressBar.value = SceneManager.progress;
            }
        }
    }
}