#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameControllerStateMachine))]
public class GameControllerStateMachine_Editor : CustomEditor_Interface
{
    protected override void DrawInspectorGUI()
    {
        GameControllerStateMachine targetScript = (GameControllerStateMachine) target;

        if (Application.isPlaying)
        {
            switch (targetScript.CurrentStateInterface)
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
        }
    }
}

#endif