#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using Data.Input;
using StateMachine;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameControllerStateMachine))]
public class GameControllerStateMachine_Editor : CustomEditor_Interface
{
    private GameControllerStateMachine _targetScript;
    private bool _playLightTest;
    private int _testCount;





    private void OnEnable()
    {
        _targetScript = (GameControllerStateMachine) target;
    }

    
    
    

    protected override void DrawInspectorGUI()
    {
        DrawBaseProperties();
        DrawNavigationContent();

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawBaseProperties()
    {
        EditorGUILayout.LabelField("Properties:");
        CustomEditorUtilities.IndentationScope(() =>
        {
            SerializedProperty sessionFormatProperty = serializedObject.FindProperty("SessionFormatObject");
            EditorGUILayout.ObjectField(sessionFormatProperty, typeof(SessionFormat));
        });
    }
    
    private void DrawNavigationContent()
    {
        if (Application.isPlaying)
        {
            if (GUILayout.Button("Automatic Light Testing"))
            {
                if (_playLightTest)
                {
                    _playLightTest = false;
                }
                else
                {
                    _targetScript.StartCoroutine(LightTest());
                }
            }
            
            
            EditorGUILayout.LabelField("Navigation:");
            CustomEditorUtilities.IndentationScope(() =>
            {
                switch (_targetScript.CurrentState)
                {
                    case State_SessionStart stateSessionStart:
                    {
                        if (GUILayout.Button("Start Session"))
                        {
                            State_SessionStart.StartSession.Invoke(_targetScript);
                        }
                    } break;
                    case State_BlockStart stateBlockStart:
                    {
                        if (GUILayout.Button("Start Block"))
                        {
                            State_BlockStart.StartBlock.Invoke(_targetScript);
                        }
                    } break;
                    case State_TrialStart stateTrialStart:
                    {
                        if (GUILayout.Button("Start Trial"))
                        {
                            State_TrialStart.StartTrial.Invoke(_targetScript);
                        }
                    } break;
                    case State_LightsOn stateLightsOn:
                    case State_AwaitingForRoomEnter stateAwaitingForRoomEnter:
                    case State_AwaitingObjectPlacement stateAwaitingObjectPlacement:
                    case State_AwaitingReturnToCorridor stateAwaitingReturnToCorridor:
                    {
                        if (GUILayout.Button("End Trial Early"))
                        {
                            _targetScript.CurrentState = _targetScript.TrialComplete;
                        }
                    } break;
                }
            });
            
            CustomEditorUtilities.Button("Stop Session Early", () =>
            {
                Debug.Log("Stopping Session Early");
                _targetScript.CurrentState = _targetScript.SessionComplete;
                
                
                
                
                
            });
        }
    }

    private IEnumerator LightTest()
    {
        _testCount = 0;
        _playLightTest = true;
        while (_playLightTest)
        {
            State_TrialStart.StartTrial.Invoke(_targetScript);
            yield return new WaitForSeconds(1f);

            _testCount++;
            _targetScript.CurrentState = _targetScript.TrialComplete;
            yield return new WaitForSeconds(1f);

            if (_targetScript.trialIndex > 20) _targetScript.trialIndex = 0;
            Debug.Log($"Test Count: <b><color=#FF0000>{_testCount}</color></b>");
            Debug.Log($"Trial: <b><color=#880000>{_targetScript.trialIndex}</color></b>");
        }
    }
}

#endif