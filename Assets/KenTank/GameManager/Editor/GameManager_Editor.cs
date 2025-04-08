namespace KenTank.GameManager.Editor
{
    using UnityEngine;
    using UnityEditor;
    using UnityEngine.Events;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(GameManager))]
    public class GameManager_Editor : Editor
    {
        const int priority = 10;

        SceneManager sceneManager;

        GUIStyle style_title => new GUIStyle(EditorStyles.boldLabel) {};

        [MenuItem("GameObject/KenTank/GameManager/GameManager", false, priority)]
        static void CreateGameManager()
        {
            string prefab = "GameManager/GameManager";
            GameObject instance = Resources.Load<GameObject>(prefab);
            GameObject go = PrefabUtility.InstantiatePrefab(instance) as GameObject;
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }

        public override VisualElement CreateInspectorGUI()
        {
            sceneManager = ((GameManager)target).GetComponentInChildren<SceneManager>(true);
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (sceneManager != null)
            {
                EditorGUILayout.BeginVertical(new GUIStyle("box"){
                    border = new RectOffset(0, 0, 2, 0),
                });
                EditorGUILayout.LabelField("Scene Options", style_title);
                EditorGUILayout.Space();
                Undo.RecordObject(sceneManager, "Change SceneManager");
                sceneManager.sceneTransition = (SceneTransition)EditorGUILayout.ObjectField("Transition", sceneManager.sceneTransition, typeof(SceneTransition), false);
                sceneManager.sceneMusic = (AudioClip)EditorGUILayout.ObjectField("Music", sceneManager.sceneMusic, typeof(AudioClip), false);
                sceneManager.volume = EditorGUILayout.Slider("Volume", sceneManager.volume, 0, 1);
                sceneManager.loop = EditorGUILayout.Toggle("Loop Music", sceneManager.loop);
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Scene Manager Unidentified!", MessageType.Warning);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(sceneManager);
            }
        }
    }
}