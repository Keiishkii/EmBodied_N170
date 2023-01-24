using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// The CustomEditor_Interface class is an interface class for custom editors.
/// It allows for the displaying of base inspector data with a button, without the need for repeated code in each custom editor class. 
/// </summary>
public abstract class CustomEditor_Interface : Editor
{
    // Variable for showing the editor windows base inspector (This is for serialised fields for the target class)
    private bool _drawBaseInspector;
    
    public override void OnInspectorGUI()
    {
        DrawInspectorGUI();
        
        EditorGUILayout.Space();
        if (GUILayout.Button(((_drawBaseInspector) ? "Hide base class inspector" : "Show base class inspector"))) _drawBaseInspector = !_drawBaseInspector;
        if (_drawBaseInspector) base.OnInspectorGUI();
    }

    // The override function for all child class inheriting from CustomEditor_Interface, used to render inspector data.
    protected abstract void DrawInspectorGUI();
}
#endif
