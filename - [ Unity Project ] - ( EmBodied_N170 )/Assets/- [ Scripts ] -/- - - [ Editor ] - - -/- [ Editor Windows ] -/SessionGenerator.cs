#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using Data.Input;
using Enums;
using Questionnaire;
using Questionnaire.Enums;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SessionGenerator : EditorWindow
{
    [SerializeField]
    private SessionGeneratorSerializedData _serializedData;
    private SerializedObject _serializedObject;

    private Vector2 _scroll;
    
    
    
    [MenuItem ("Bournemouth University/Session Generator")]
    public static void  ShowWindow () 
    {
        GetWindow(typeof(SessionGenerator));
    }

    private void OnEnable()
    {
        hideFlags = HideFlags.HideAndDontSave;

        _serializedData = AssetDatabase.LoadAssetAtPath<SessionGeneratorSerializedData>("Assets/Editor/Session Generator.asset");
        if (!_serializedData)
        {
            Debug.Log("Could not find asset: Session Generator");
            
            _serializedData = ScriptableObject.CreateInstance<SessionGeneratorSerializedData>();
            AssetDatabase.CreateAsset(_serializedData, $"Assets/Editor/Session Generator.asset");
        }

        _serializedObject = new SerializedObject(_serializedData);
    }

    private void OnLostFocus()
    {
        AssetDatabase.SaveAssets();
    }


    private void OnGUI()
    {
        CustomEditorUtilities.ScrollScope(ref _scroll, () =>
        {
            DrawNPCPrefabLists();
            DrawGenerationData();
            DrawGenerateButton();
        });
        
        _serializedObject.ApplyModifiedProperties();
    }

    private void DrawGenerationData()
    {
        EditorGUILayout.LabelField("Generation Data: ");
        
        _serializedObject.FindProperty("approachDistance").floatValue = EditorGUILayout.FloatField("NPC Look at Distance: ", _serializedObject.FindProperty("approachDistance").floatValue);
        _serializedObject.FindProperty("participantHandedness").enumValueIndex = (int) (Handedness) EditorGUILayout.EnumPopup("Participant Handedness: ", (Handedness) _serializedObject.FindProperty("participantHandedness").enumValueIndex);
        _serializedObject.FindProperty("initialTargetRoom").enumValueIndex = (int) (Room) EditorGUILayout.EnumPopup("Initial Target Room: ", (Room) _serializedObject.FindProperty("initialTargetRoom").enumValueIndex);
        
        EditorGUILayout.Space(5f);
        
        _serializedObject.FindProperty("heldItem").objectReferenceValue = EditorGUILayout.ObjectField($"Held Item: ", _serializedObject.FindProperty("heldItem").objectReferenceValue, typeof(GameObject), false);
        
        EditorGUILayout.Space(5f);
        
        _serializedObject.FindProperty("blocks").intValue = EditorGUILayout.IntField("Blocks: ", _serializedObject.FindProperty("blocks").intValue);
        _serializedObject.FindProperty("trials").intValue = EditorGUILayout.IntField("Trials: ", _serializedObject.FindProperty("trials").intValue);
        
        EditorGUILayout.Space(5f);
        
        _serializedObject.FindProperty("questionOne").stringValue = EditorGUILayout.TextField("Question One", _serializedObject.FindProperty("questionOne").stringValue);
        _serializedObject.FindProperty("questionOneLowTexture").objectReferenceValue = EditorGUILayout.ObjectField($"Low Answer: ", _serializedObject.FindProperty("questionOneLowTexture").objectReferenceValue, typeof(Texture2D), false);
        _serializedObject.FindProperty("questionOneHighTexture").objectReferenceValue = EditorGUILayout.ObjectField($"High Answer: ", _serializedObject.FindProperty("questionOneHighTexture").objectReferenceValue, typeof(Texture2D), false);
        
        _serializedObject.FindProperty("questionTwo").stringValue = EditorGUILayout.TextField("Question Two", _serializedObject.FindProperty("questionTwo").stringValue);
        _serializedObject.FindProperty("questionTwoLowTexture").objectReferenceValue = EditorGUILayout.ObjectField($"Low Answer: ", _serializedObject.FindProperty("questionTwoLowTexture").objectReferenceValue, typeof(Texture2D), false);
        _serializedObject.FindProperty("questionTwoHighTexture").objectReferenceValue = EditorGUILayout.ObjectField($"High Answer: ", _serializedObject.FindProperty("questionTwoHighTexture").objectReferenceValue, typeof(Texture2D), false);
    }
    
    private void DrawGenerateButton()
    {
        EditorGUILayout.LabelField("Generate: ");
        
        EditorGUILayout.Space(25f);

        SerializedProperty fileDirectoryProperty = _serializedObject.FindProperty("fileDirectory");
        SerializedProperty filenameProperty = _serializedObject.FindProperty("filename");
        
        fileDirectoryProperty.stringValue = EditorGUILayout.TextField("File Directory: ", fileDirectoryProperty.stringValue);
        filenameProperty.stringValue = EditorGUILayout.TextField("Filename: ", filenameProperty.stringValue);
        EditorGUILayout.LabelField("     - path is relative to project directory");
        
        
        EditorGUILayout.Space(5f);
        
        CustomEditorUtilities.Button("Generate Session", () =>
        {
            SessionFormat sessionFormat = CreateInstance<SessionFormat>();
            SerializedObject sessionSerializedObject = new SerializedObject(sessionFormat);

            
            sessionSerializedObject.FindProperty("approachDistance").floatValue = _serializedObject.FindProperty("approachDistance").floatValue;
            sessionSerializedObject.FindProperty("participantHandedness").enumValueIndex = _serializedObject.FindProperty("participantHandedness").enumValueIndex;


            SerializedProperty blocks = sessionSerializedObject.FindProperty("blocks");
            blocks.arraySize = 0;
            
            //  - [ Blocks ] - 
            #region Blocks
            Room targetRoom = (Room) _serializedObject.FindProperty("initialTargetRoom").enumValueIndex;
            for (int i = 0; i < _serializedObject.FindProperty("blocks").intValue; i++)
            {
                blocks.arraySize++;
                SerializedProperty block = blocks.GetArrayElementAtIndex(i);
                block.managedReferenceValue = new Block();

                block.FindPropertyRelative("targetRoom").enumValueIndex = (int) targetRoom;
                targetRoom = (targetRoom == Room.RoomA) ? Room.RoomB : Room.RoomA;

                //  - [ Trials ] - 
                #region Trials
                SerializedProperty trials = block.FindPropertyRelative("trials");
                trials.arraySize = 0;
                for (int j = 0; j < _serializedObject.FindProperty("trials").intValue; j++)
                {
                    trials.arraySize++;
                    SerializedProperty trial = trials.GetArrayElementAtIndex(j);
                    trial.managedReferenceValue = new Trial();

                    //  - [ Held Objects ] - 
                    #region Non Player Characters
                    trial.FindPropertyRelative("heldObject").objectReferenceValue = _serializedObject.FindProperty("heldItem").objectReferenceValue;
                    #endregion

                    //  - [ Non Player Characters ] - 
                    #region Non Player Characters
                    int typeRandomValue = Random.Range(0, 2);
                    if (typeRandomValue > 0) // Statues Selected
                    {
                        SerializedProperty statuesAList = _serializedObject.FindProperty("groupAStatues").FindPropertyRelative("prefabList");
                        SerializedProperty statuesBList = _serializedObject.FindProperty("groupBStatues").FindPropertyRelative("prefabList");
                        
                        int statueARandomIndex = Random.Range(0, statuesAList.arraySize);
                        int statueBRandomIndex = Random.Range(0, statuesBList.arraySize);
                        
                        trial.FindPropertyRelative("roomA_NPCAvatar").objectReferenceValue = statuesAList.GetArrayElementAtIndex(statueARandomIndex).objectReferenceValue;
                        trial.FindPropertyRelative("roomB_NPCAvatar").objectReferenceValue = statuesBList.GetArrayElementAtIndex(statueBRandomIndex).objectReferenceValue;
                    }
                    else // Normal NPCs selected
                    {
                        SerializedProperty npcAList = _serializedObject.FindProperty("groupA").FindPropertyRelative("prefabList");
                        SerializedProperty npcBList = _serializedObject.FindProperty("groupB").FindPropertyRelative("prefabList");
                        
                        int npcARandomIndex = Random.Range(0, npcAList.arraySize);
                        int npcBRandomIndex = Random.Range(0, npcBList.arraySize);
                        
                        trial.FindPropertyRelative("roomA_NPCAvatar").objectReferenceValue = npcAList.GetArrayElementAtIndex(npcARandomIndex).objectReferenceValue;
                        trial.FindPropertyRelative("roomB_NPCAvatar").objectReferenceValue = npcBList.GetArrayElementAtIndex(npcBRandomIndex).objectReferenceValue;
                    }
                    #endregion
                }
                #endregion
                
                //  - [ Questionnaire ] - 
                #region Questionnaire
                SerializedProperty questionnairePanelsProperty = block.FindPropertyRelative("blockQuestionnairePanels");
                questionnairePanelsProperty.arraySize = 0;

                questionnairePanelsProperty.arraySize++;
                SerializedProperty questionnairePanel = questionnairePanelsProperty.GetArrayElementAtIndex(0);
                questionnairePanel.managedReferenceValue = new QuestionnairePanel_TwoSliders();

                questionnairePanel.FindPropertyRelative("questionnairePanelType").enumValueIndex = (int) (QuestionnairePanelType_Enum.TwoQuestion_SliderAnswer);
                
                questionnairePanel.FindPropertyRelative("sliderOneQuestion").stringValue = _serializedObject.FindProperty("questionOne").stringValue;
                questionnairePanel.FindPropertyRelative("sliderTwoQuestion").stringValue = _serializedObject.FindProperty("questionTwo").stringValue;
                
                questionnairePanel.FindPropertyRelative("sliderOneQuestionDecorType").enumValueIndex = (int) (Enums.QuestionDecor_Enum.Image);
                questionnairePanel.FindPropertyRelative("sliderTwoQuestionDecorType").enumValueIndex = (int) (Enums.QuestionDecor_Enum.Image);
                
                questionnairePanel.FindPropertyRelative("sliderOneQuestionDecorMinimumSprite").objectReferenceValue = _serializedObject.FindProperty("questionOneLowTexture").objectReferenceValue;
                questionnairePanel.FindPropertyRelative("sliderOneQuestionDecorMaximumSprite").objectReferenceValue = _serializedObject.FindProperty("questionOneHighTexture").objectReferenceValue;
                questionnairePanel.FindPropertyRelative("sliderTwoQuestionDecorMinimumSprite").objectReferenceValue = _serializedObject.FindProperty("questionTwoLowTexture").objectReferenceValue;
                questionnairePanel.FindPropertyRelative("sliderTwoQuestionDecorMaximumSprite").objectReferenceValue = _serializedObject.FindProperty("questionTwoHighTexture").objectReferenceValue;
                #endregion
            }
            #endregion
            
            
            sessionSerializedObject.ApplyModifiedProperties();
            
            
            if (!Directory.Exists(fileDirectoryProperty.stringValue)) { Directory.CreateDirectory(fileDirectoryProperty.stringValue); }
            AssetDatabase.CreateAsset(sessionFormat, $"{fileDirectoryProperty.stringValue}/{filenameProperty.stringValue}.asset");
        });
    }
    
    private void DrawNPCPrefabLists()
    {
        EditorGUILayout.LabelField("Non Player Character - Prefabs: ");
        
        DrawPrefabList(_serializedObject.FindProperty("groupA"), "Group A: ");
        DrawPrefabList(_serializedObject.FindProperty("groupAStatues"), "Group A Statues: ");
        DrawPrefabList(_serializedObject.FindProperty("groupB"), "Group B: ");
        DrawPrefabList(_serializedObject.FindProperty("groupBStatues"), "Group B Statues: ");
    }

    private void DrawPrefabList(SerializedProperty groupProperty, in string label)
    {
        CustomEditorUtilities.CollapsableScope(groupProperty.FindPropertyRelative("listVisibility"), label, () =>
        {
            SerializedProperty listProperty = groupProperty.FindPropertyRelative("prefabList");
            SerializedProperty countProperty = groupProperty.FindPropertyRelative("listCount");
                        
            CustomEditorUtilities.RenderObjectList(countProperty, listProperty, 
                (() =>
                {
                    listProperty.arraySize++;
                    listProperty.GetArrayElementAtIndex(listProperty.arraySize - 1).objectReferenceValue = null;
                }),
                ((index) =>
                {
                    SerializedProperty objectProperty = listProperty.GetArrayElementAtIndex(index);
                    objectProperty.objectReferenceValue = EditorGUILayout.ObjectField($"Avatar {index}: ", objectProperty.objectReferenceValue, typeof(GameObject), false);
                }));
                
        });
        }
    }
#endif