using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace KenTank.GameManager
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance => GameManager.instance.audioManager;

        public AudioMixer mixer;
        public AudioSource music;
        public AudioSource ambient;
        public AudioSource effect;
        public AudioSource voice;
        public AudioSource ui;

        public AudioSource CreateInstance(AudioSource source, AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = Instantiate(source);
            DontDestroyOnLoad(item.gameObject);
            item.clip = clip;
            item.volume = 0;
            item.Play();
            item!.DOFade(volume, fadeIn).SetTarget(item.gameObject);
            var time = 0.00f;
            DOTween.To(x => time = x, time, 1, clip.length - (fadeIn + fadeOut)).SetTarget(item.gameObject)
            .onComplete = () => {
                item!.DOFade(0, fadeOut).SetTarget(item.gameObject).onComplete = () => {
                    onComplete?.Invoke();
                    Destroy(item.gameObject);
                };
            };
            return item;
        }

        public AudioSource CreateMusicInstance(AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = CreateInstance(instance.music, clip, volume, fadeIn, fadeOut, onComplete);
            return item;
        }

        public AudioSource CreateAmbientInstance(AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = CreateInstance(instance.ambient, clip, volume, fadeIn, fadeOut, onComplete);
            return item;
        }
        
        public AudioSource CreateEffectInstance(AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = CreateInstance(instance.effect, clip, volume, fadeIn, fadeOut, onComplete);
            return item;
        }

        public AudioSource CreateVoiceInstance(AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = CreateInstance(instance.voice, clip, volume, fadeIn, fadeOut, onComplete);
            return item;
        }

        public AudioSource CreateUIInstance(AudioClip clip, float volume = 1, float fadeIn = 0, float fadeOut = 0, Action onComplete = null)
        {
            var item = CreateInstance(instance.ui, clip, volume, fadeIn, fadeOut, onComplete);
            return item;
        }
    }
}