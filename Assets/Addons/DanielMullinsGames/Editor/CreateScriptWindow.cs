using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateScriptWindow : EditorWindow
{
    private static string destinationPath;
    private bool focusedOnWindowCreated = false;
    
    private string nameInput = "";
    private int templateIndex = 0;
    private string[] templatePaths;

    [MenuItem("Assets/Create/Script From Template %#x", false, 0)] // CMD + SHIFT + X
    private static void CreateManagedBehaviourScript()
    {
        SetDestinationPath();

        var window = CreateInstance(typeof(CreateScriptWindow)) as CreateScriptWindow;
        window.titleContent.text = "Create Script From Template";
        window.ShowUtility();
    }

    private static void SetDestinationPath()
    {
        destinationPath = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            destinationPath = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(destinationPath))
            {
                destinationPath = Path.GetDirectoryName(destinationPath);
            }
            break;
        }
    }

    private static void CreateScriptFromTemplate(string newScriptName, string templatePath)
    {
        string templateText = File.ReadAllText(templatePath);
        templateText = templateText.Replace("#SCRIPTNAME#", newScriptName);

        string newFilePath = destinationPath + "\\" + newScriptName + ".cs";
        using (var file = new StreamWriter(newFilePath))
        {
            file.Write(templateText);
        }

        AssetDatabase.Refresh();
    }

    private void PopulateTemplates()
    {
        templatePaths = Directory.GetFiles(Application.dataPath + "/Addons/DanielMullinsGames/ScriptTemplates");
    }

    private string FormatFilePathForDisplay(string filePath)
    {
        int startIndex = filePath.LastIndexOf('\\');
        int endIndex = Mathf.Min(filePath.LastIndexOf('.'), filePath.LastIndexOf("Template"));
        return filePath.Substring(startIndex + 1, endIndex - startIndex - 1);
    }

    private void OnGUI()
    {
        if (templatePaths == null)
        {
            PopulateTemplates();
        }

        int cachedFontSize = GUI.skin.textField.fontSize;
        GUI.skin.textField.fontSize = 30;

        EditorGUILayout.LabelField("Create script in: " + destinationPath);

        GUI.SetNextControlName("ScriptName");
        nameInput = EditorGUILayout.TextField(nameInput, GUILayout.Height(40));

        string[] formattedNames = new string[templatePaths.Length];
        for (int i = 0; i < templatePaths.Length; i++)
        {
            formattedNames[i] = FormatFilePathForDisplay(templatePaths[i]);
        }

        templateIndex = EditorGUILayout.Popup(templateIndex, formattedNames);

        GUI.skin.textField.fontSize = cachedFontSize;
        if (Event.current != null && Event.current.isKey && Event.current.keyCode == KeyCode.Return)
        {
            if (!string.IsNullOrEmpty(nameInput))
            {
                CreateScriptFromTemplate(nameInput, templatePaths[templateIndex]);
                Close();
            }
            else
            {
                Close();
            }
        }

        if (!focusedOnWindowCreated)
        {
            GUI.FocusControl("ScriptName");
            focusedOnWindowCreated = true;
        }
    }
}
