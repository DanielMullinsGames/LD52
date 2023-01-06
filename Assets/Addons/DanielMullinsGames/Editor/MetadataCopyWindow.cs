using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace DanielMullinsGames
{
    public class MetadataCopyWindow : EditorWindow
    {
        const string GUID_TAG = "guid: ";

        private Object objFrom;
        private Object objTo;

        private bool copied;
        private bool wasCompiling;

        [MenuItem("Tools/Daniel Mullins Games/Copy Metadata")]
        public static void ShowWindow()
        {
            GetWindow(typeof(MetadataCopyWindow));
        }

        void OnGUI()
        {
            //HANDLE OPENING WHILE COMPILING
            if (EditorApplication.isCompiling)
            {
                GUILayout.Label("Compiling...", EditorStyles.boldLabel);
                wasCompiling = true;
                return;
            }
            else if (wasCompiling)
            {
                wasCompiling = false;
                Close();
                ShowWindow();
                return;
            }

            //DRAW UI
            EditorGUILayout.BeginVertical();
            {
                objFrom = EditorGUI.ObjectField(new Rect(3, 33, position.width - 6, 20), "Copy From", objFrom, typeof(Object), allowSceneObjects: false);
                objTo = EditorGUI.ObjectField(new Rect(3, 60, position.width - 6, 20), "Copy To", objTo, typeof(Object), allowSceneObjects: false);

                if (objFrom != null && objTo != null)
                {
                    if (GUILayout.Button(copied ? "SUCCESS! COPY AGAIN?" : "COPY", GUILayout.Width(position.width - 6), GUILayout.Height(20)))
                    {
                        CopyMetadata();
                    }
                }
                else
                {
                    GUILayout.Label("Drag files into the 'To' and 'From' fields.", EditorStyles.boldLabel);
                }

            }
            EditorGUILayout.EndVertical();
        }

        private void CopyMetadata()
        {
            string pathFrom = AssetDatabase.GetAssetPath(objFrom);
            string pathTo = AssetDatabase.GetAssetPath(objTo);

            FileUtil.DeleteFileOrDirectory(pathTo + ".meta");
            FileUtil.CopyFileOrDirectory(pathFrom + ".meta", pathTo + ".meta");

            // Create a new GUID
            WriteGuidToMetaFile(pathTo + ".meta", GetGuidFromMetaFile(pathTo + ".meta"), System.Guid.NewGuid().ToString());

            AssetDatabase.ImportAsset(pathTo, ImportAssetOptions.ForceUpdate);

            copied = true;
        }

        private void WriteGuidToMetaFile(string path, string oldGuid, string newGuid)
        {
            string fullText = File.ReadAllText(path);
            fullText = fullText.Replace(oldGuid, newGuid);
            File.WriteAllText(path, fullText);
        }

        private string GetGuidFromMetaFile(string path)
        {
            StreamReader fileReader = new StreamReader(path);
            string fullText = fileReader.ReadToEnd();
            fileReader.Close();

            using (var reader = new StringReader(fullText))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(GUID_TAG))
                    {
                        int startIndex = line.IndexOf(GUID_TAG) + GUID_TAG.Length;
                        string guid = line.Substring(startIndex);
                        return guid;
                    }
                }
            }

            return "";
        }
    }
}
