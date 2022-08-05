#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using Data.Input;
using StateMachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameControllerStateMachine))]
public class GameControllerStateMachine_Editor : CustomEditor_Interface
{
    protected override void DrawInspectorGUI()
    {
        GameControllerStateMachine targetScript = (GameControllerStateMachine) target;

        DrawBaseProperties(targetScript);
        DrawNavigationContent(targetScript);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBaseProperties(GameControllerStateMachine targetScript)
    {
        EditorGUILayout.LabelField("Properties:");
        CustomEditorUtilities.IndentationScope(() =>
        {
            SerializedProperty sessionFormatProperty = serializedObject.FindProperty("SessionFormatObject");
            EditorGUILayout.ObjectField(sessionFormatProperty, typeof(SessionFormat));
        });
    }
    
    private void DrawNavigationContent(GameControllerStateMachine targetScript)
    {
        if (Application.isPlaying)
        {
            EditorGUILayout.LabelField("Navigation:");
            CustomEditorUtilities.IndentationScope(() =>
            {
                switch (targetScript.CurrentState)
                {
                    case State_SessionStart sessionStartState:
                    {
                        if (GUILayout.Button("Start Session"))
                        {
                            State_SessionStart.StartSession.Invoke(targetScript);
                        }
                    } break;
                    case State_BlockStart blockStartState:
                    {
                        if (GUILayout.Button("Start Block"))
                        {
                            State_BlockStart.StartBlock.Invoke(targetScript);
                        }
                    } break;
                }
            });
        }
    }
}

#endif