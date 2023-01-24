#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;


/// <summary>
/// This class is a utility class for creating custom inspectors, allowing for the writing of more compact and clean inspector code.
/// Functions are called statically and supply a lambda expression to run the editor code after modification of the inputs.
/// </summary>
public static class CustomEditorUtilities
{
    // Combines the button logic into a single function call with lambda expressions.
    public static void Button(in string label, Action callback)
    {
        if (GUILayout.Button(label))
        {
            callback.Invoke();
        }
    }

    // Renders and object list where each property is displayed.
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

    // Puts the contents of the lambda expression within a scrollable list.
    public static void ScrollScope(ref Vector2 scroll, Action callback)
    {
        scroll = GUILayout.BeginScrollView(scroll);
        {
            callback.Invoke();
        }
        GUILayout.EndScrollView();
    }

    // Puts the contents of the lambda expression within a collapsable scope.
    public static void CollapsableScope(SerializedProperty visibilityProperty, in string label, Action callback)
    {
        visibilityProperty.boolValue = EditorGUILayout.Foldout(visibilityProperty.boolValue, label);
        if (visibilityProperty.boolValue)
        {
            VerticalScope(callback);
        }
    }
    
    // Puts the contents of the lambda expression inside a vertical scope.
    public static void VerticalScope(Action callback)
    {
        using (new EditorGUILayout.VerticalScope())
        {
            callback.Invoke();
        }
    }
    
    // Puts the contents of the lambda expression inside a horizontal scope.
    public static void HorizontalScope(Action callback)
    {
        using (new EditorGUILayout.VerticalScope())
        {
            callback.Invoke();
        }
    }

    // Puts the contents of the lambda expression inside an indented scope.
    public static void IndentationScope(Action callback)
    {
        EditorGUI.indentLevel++;
        callback.Invoke();
        EditorGUI.indentLevel--;
    }
}
#endif