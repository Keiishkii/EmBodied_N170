#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public static class CustomEditorUtilities
{
    public static void Button(in string label, Action callback)
    {
        if (GUILayout.Button(label))
        {
            callback.Invoke();
        }
    }

    public static void CollapsableLabel(SerializedProperty boolProperty, in string label, Action callback)
    {
        boolProperty.boolValue = EditorGUILayout.Foldout(boolProperty.boolValue, label);
        if (boolProperty.boolValue)
        {
            VerticalScope(callback);
        }
    }

    public static void RenderObjectList(SerializedProperty countProperty, SerializedProperty listProperty, Action insertion, Action<int> iteration)
    {
        EditorGUI.BeginChangeCheck();
        countProperty.intValue = EditorGUILayout.IntField("Prefab Count: ", countProperty.intValue);
        if (EditorGUI.EndChangeCheck() || true)
        {
            int count = countProperty.intValue;
            while (count > listProperty.arraySize)
            {
                insertion.Invoke();
            }

            while (count < listProperty.arraySize)
            {
                listProperty.arraySize = count;
            }
        }

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            iteration.Invoke(i);
        }
    }
    
    public static void VerticalScope(Action callback)
    {
        using (new EditorGUILayout.VerticalScope())
        {
            callback.Invoke();
        }
    }
    
    public static void HorizontalScope(Action callback)
    {
        using (new EditorGUILayout.VerticalScope())
        {
            callback.Invoke();
        }
    }
}
#endif
