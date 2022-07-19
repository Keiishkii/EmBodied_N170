using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public abstract class CustomEditor_Interface : Editor
{
    private bool _drawBaseInspector;
    
    public override void OnInspectorGUI()
    {
        DrawInspectorGUI();
        
        EditorGUILayout.Space();
        if (GUILayout.Button(((_drawBaseInspector) ? "Hide base class inspector" : "Show base class inspector"))) _drawBaseInspector = !_drawBaseInspector;
        if (_drawBaseInspector) base.OnInspectorGUI();
    }

    protected abstract void DrawInspectorGUI();
}
#endif
