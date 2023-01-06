using UnityEditor;
using System.Reflection;

static class UsefulShortcuts
{
#if UNITY_EDITOR
    [MenuItem("Tools/Daniel Mullins Games/Clear Console %#c")] // CMD + SHIFT + C
    private static void ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
#endif
}
