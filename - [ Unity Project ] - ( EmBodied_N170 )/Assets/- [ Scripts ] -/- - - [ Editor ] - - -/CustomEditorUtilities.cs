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

    public static void ScrollScope(ref Vector2 scroll, Action callback)
    {
        scroll = GUILayout.BeginScrollView(scroll);
        {
            callback.Invoke();
        }
        GUILayout.EndScrollView();
    }

    public static void CollapsableScope(SerializedProperty visibilityProperty, in string label, Action callback)
    {
        visibilityProperty.boolValue = EditorGUILayout.Foldout(visibilityProperty.boolValue, label);
        if (visibilityProperty.boolValue)
        {
            VerticalScope(callback);
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

    public static void IndentationScope(Action callback)
    {
        EditorGUI.indentLevel++;
        callback.Invoke();
        EditorGUI.indentLevel--;
    }
}
#endif