using UnityEngine;

namespace KenTank.GameManager
{
    public class AudioActions : MonoBehaviour
    {
        enum CreateType {
            Music, Effect, Ambient, Voice, UI
        }

        [Header("Options")]
        [SerializeField] CreateType createType;
        [SerializeField] AudioClip clip;
        [SerializeField] float volume = 1;
        [SerializeField] float fadeIn;
        [SerializeField] float fadeOut;
        [Tooltip("Stop Played Music, Only For Music Type")]
        [SerializeField] bool stopMusic;
        
        [Header("Behaviours")]
        [SerializeField] bool createOnEnable;

        public void CreateAudio(AudioClip clip)
        {
            if (clip == null) return;

            switch (createType)
            {
                case CreateType.Music:
                if (stopMusic) SceneManager.StopCurrentMusic();
                AudioManager.instance.CreateMusicInstance(clip, volume, fadeIn, fadeOut);
                break;

                case CreateType.Effect:
                AudioManager.instance.CreateEffectInstance(clip, volume, fadeIn, fadeOut);
                break;

                case CreateType.Ambient:
                AudioManager.instance.CreateAmbientInstance(clip, volume, fadeIn, fadeOut);
                break;

                case CreateType.Voice:
                AudioManager.instance.CreateVoiceInstance(clip, volume, fadeIn, fadeOut);
                break;

                case CreateType.UI:
                AudioManager.instance.CreateUIInstance(clip, volume, fadeIn, fadeOut);
                break;
            }
        }

        public void CreateAudio() 
        {
            CreateAudio(clip);
        }

        void OnEnable()
        {
            if (createOnEnable)
            {
                CreateAudio();
            }
        }
    }
}
