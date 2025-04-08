using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Audio;

namespace KenTank.GameManager
{
    public class SettingsManager : MonoBehaviour
    {
        public static Dictionary<string,object> settings = new Dictionary<string, object>{
            {"display-framerate", 0},
            {"display-vsync", 0},
            {"audio-master-volume", 1},
            {"audio-music-volume", 1},
            {"audio-ambient-volume", 1},
            {"audio-effect-volume", 1},
            {"audio-voice-volume", 1},
            {"audio-ui-volume", 1},
        };

        [Header("Behaviours")]
        [SerializeField] AudioMixer _mixer;

        public static string path => Path.Join(Application.persistentDataPath, "settings.json");
        public static AudioMixer mixer;

        static float delaySaveTime = 0;

        public static async Task SaveAsync() 
        {
            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            using StreamWriter writer = new StreamWriter(path);
            await writer.WriteAsync(json);
            writer.Dispose();
            Debug.Log("Settings Saved!");
        }

        public static async Task LoadAsync() 
        {
            if (!File.Exists(path))
            {
                await SaveAsync();
                return;
            }
            using StreamReader reader = new StreamReader(path);
            var json = JsonConvert.DeserializeObject<Dictionary<string,object>>(await reader.ReadToEndAsync());
            bool newFieldUpdate = false;
            if (json.Count != settings.Count)
            {
                foreach (var item in json)
                {
                    if (settings.ContainsKey(item.Key))
                    {
                        settings[item.Key] = item.Value;
                    }
                }
                newFieldUpdate = true;
            }
            else
            {
                settings = json;
            }
            reader.Dispose();
            Debug.Log("Settings Loaded");
            if (newFieldUpdate)
            {
                Debug.Log("Found new field update!");
                await SaveAsync();
            }
        }

        public static async void Save() 
        {
            await SaveAsync();
        }

        public static async void Load() 
        {
            await LoadAsync();
        }

        public static void DelaySave() 
        {
            delaySaveTime = 5;
        }

        public void Init() 
        {
            StartCoroutine(InitCoroutine());
        }

        IEnumerator InitCoroutine() 
        {
            mixer = _mixer;
            var task = LoadAsync();
            while (!task.IsCompleted)
            {
                yield return null;
            }
            Settings.Init();
            Debug.Log("Settings Initialization Complete!");
        }

        void Update()
        {
            if (delaySaveTime > 0)
            {
                delaySaveTime -= Time.unscaledDeltaTime;
                if (delaySaveTime <= 0)
                {
                    Save();
                }
            }
        }
    }

    public class Settings 
    {
        static bool ready = false;

        public static void Save() 
        {
            if (ready) SettingsManager.DelaySave();
        }
        
        public class Display 
        {   
            public static int[] Framerates = new int[] {
                999, 30, 60, 75, 90, 100, 120, 144
            };

            public static int Framerate {
                get {
                    return Convert.ToInt32(SettingsManager.settings["display-framerate"]);
                }
                set {
                    SettingsManager.settings["framerate"] = value;
                    Save();
                    Application.targetFrameRate = Framerates[value];
                    QualitySettings.vSyncCount = 0;
                }
            }

            public static int VSync {
                get {
                    return Convert.ToInt32(SettingsManager.settings["display-vsync"]);
                }
                set {
                    SettingsManager.settings["vsync"] = value;
                    Save();
                    QualitySettings.vSyncCount = value;
                }
            }
        }

        public class Audio 
        {
            public class Master 
            {
                const string path = "audio-master-volume";
                const string volume_key = "master-volume";
                
                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
            public class Music 
            {
                const string path = "audio-music-volume";
                const string volume_key = "music-volume";

                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
            public class Ambient 
            {
                const string path = "audio-ambient-volume";
                const string volume_key = "ambient-volume";

                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
            public class Effect 
            {
                const string path = "audio-effect-volume";
                const string volume_key = "effect-volume";

                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
            public class Voice 
            {
                const string path = "audio-voice-volume";
                const string volume_key = "voice-volume";

                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
            public class UI 
            {
                const string path = "audio-ui-volume";
                const string volume_key = "ui-volume";

                public static float Volume {
                    get {
                        return Convert.ToSingle(SettingsManager.settings[path]);
                    }
                    set {
                        SettingsManager.settings[path] = value;
                        Save();
                        var v = Mathf.Clamp01(value);
                        v = v > 0 ? Mathf.Log10(v) * 20f : -80f;
                        SettingsManager.mixer.SetFloat(volume_key, v);
                    }
                }
            }
        }

        public static void Init()
        {
            Display.Framerate = Display.Framerate;
            Display.VSync = Display.VSync;
            Audio.Master.Volume = Audio.Master.Volume;
            Audio.Music.Volume = Audio.Music.Volume;
            Audio.Effect.Volume = Audio.Effect.Volume;
            Audio.Ambient.Volume = Audio.Ambient.Volume;
            Audio.Voice.Volume = Audio.Voice.Volume;
            Audio.UI.Volume = Audio.UI.Volume;
            ready = true;
        }
    }
}
