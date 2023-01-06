using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

namespace DanielMullinsGames
{
    public class SceneSearchWindow : EditorWindow
    {
        private List<string> sceneNames = new List<string>();

        private string searchInput = "";
        private bool focusedOnField = false;

        private static string ScenesPath { get { return Application.dataPath + "/Scenes/"; } } //< -- Set Scene Path Here

        [MenuItem("Tools/Daniel Mullins Games/Scene Search")]
        public static void ShowWindow()
        {
            var window = CreateInstance(typeof(SceneSearchWindow)) as SceneSearchWindow;
            window.titleContent.text = "Scene Search";
            window.ShowUtility();
        }

        private void Awake()
        {
            foreach (string file in Directory.EnumerateFiles(ScenesPath, "*.unity", SearchOption.AllDirectories))
            {
                string trimmedName = file.Replace(ScenesPath, "").Replace(".unity", "");
                sceneNames.Add(trimmedName);
            }
        }

        private void OnGUI()
        {
            int cachedFontSize = GUI.skin.textField.fontSize;
            GUI.skin.textField.fontSize = 30;
            GUI.SetNextControlName("SearchField");
            searchInput = EditorGUILayout.TextField(searchInput, GUILayout.Height(40));
            GUI.skin.textField.fontSize = cachedFontSize;

            string match = null;
            if (!string.IsNullOrEmpty(searchInput))
            {
                match = sceneNames.Find(x => SearchInputMatches(searchInput, x));
                if (match != null)
                {
                    DrawMatch(match);
                }
                else
                {
                    match = sceneNames.Find(x => SearchInputMatches(searchInput, x));
                    if (match != null)
                    {
                        DrawMatch(match);
                    }
                }
            }

            if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
            {
                if (match != null)
                {
                    LoadScene(match);
                }
                else
                {
                    Close();
                }
            }

            if (!focusedOnField)
            {
                GUI.FocusControl("SearchField");
                focusedOnField = true;
            }
        }

        private bool SearchInputMatches(string searchInput, string sceneName)
        {
            return GetSceneName(sceneName).Replace("_", "").Replace("-", "").Replace("Screen", "").ToLower().StartsWith(searchInput.ToLower());
        }

        private void DrawMatch(string match)
        {
            EditorGUILayout.LabelField(match);
            EditorGUILayout.LabelField("(press Enter to load)");
        }

        private string GetSceneName(string fileName)
        {
            if (fileName.Contains("\\"))
            {
                int slashIndex = fileName.LastIndexOf("\\");
                string subString = fileName.Substring(slashIndex + 1, fileName.Length - slashIndex - 1);
                return subString;
            }
            return fileName;
        }

        private void LoadScene(string sceneName)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(ScenesPath + sceneName + ".unity", OpenSceneMode.Single);
                Close();
            }
        }
    }
}
