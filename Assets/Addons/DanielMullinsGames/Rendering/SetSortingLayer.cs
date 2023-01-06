using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
#endif

[RequireComponent(typeof(Renderer))]
public class SetSortingLayer : MonoBehaviour
{
    [HideInInspector]
    public int sortingLayerIndex;
    public int sortingOrder;

    private int additiveSortingOrder;

    public void ApplySorting(string sortingLayer, int sortingOrder)
    {
        GetComponent<Renderer>().sortingLayerName = sortingLayer;
        GetComponent<Renderer>().sortingOrder = sortingOrder;
    }

    public void SetAdditiveSortingOrder(int order)
    {
        additiveSortingOrder = order;
        GetComponent<Renderer>().sortingOrder = sortingOrder + additiveSortingOrder;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SetSortingLayer))]
    public class SetSortingLayerEditor : Editor
    {
        string[] _choices;

        public override void OnInspectorGUI()
        {
            var sorter = target as SetSortingLayer;

            DrawDefaultInspector();

            _choices = GetSortingLayerNames();
            sorter.sortingLayerIndex = EditorGUILayout.Popup(sorter.sortingLayerIndex, _choices);

            if (_choices.Length > 0)
            {
                sorter.ApplySorting(_choices[sorter.sortingLayerIndex], sorter.sortingOrder);
            }

            if (GUILayout.Button("Apply"))
            {
                EditorUtility.SetDirty(target);
            }
        }

        public string[] GetSortingLayerNames()
        {
            var internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }
    }
#endif
}