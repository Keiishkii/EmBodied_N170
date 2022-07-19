#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enums;
using Questionnaire;
using Questionnaire.Enums;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

namespace Data {
    namespace Input
    {
        [CustomEditor(typeof(SessionFormat))]
        public class SessionFormat_Editor : Editor
        {
            private GUISkin _editorSkin;

            private GUISkin EditorSkin
            {
                get
                {
                    if (ReferenceEquals(_editorSkin, null))
                    {
                        _editorSkin = Resources.Load<GUISkin>("GUISkin");
                    }

                    return _editorSkin;
                }
            }

            private SerializedProperty _approachDistanceProperty;
            private SerializedProperty _participantHandednessProperty;
            private SerializedProperty _blockListProperty;





            private void OnEnable()
            {
                _approachDistanceProperty = serializedObject.FindProperty("approachDistance");
                _participantHandednessProperty = serializedObject.FindProperty("participantHandedness");

                _blockListProperty = serializedObject.FindProperty("blocks");
            }





            public override void OnInspectorGUI()
            {
                GUI.skin = EditorSkin;


                // ---- Session Data

                #region Session Data

                using (new EditorGUILayout.VerticalScope(EditorSkin.customStyles[0]))
                {
                    _approachDistanceProperty.floatValue = EditorGUILayout.FloatField("Approach Distance: ", _approachDistanceProperty.floatValue);
                    _participantHandednessProperty.enumValueIndex = (int) (Enums.Handedness) EditorGUILayout.EnumPopup("Handedness: ", (Enums.Handedness) _participantHandednessProperty.enumValueIndex);
                }

                #endregion

                // ---- Render Blocks

                #region Render Blocks

                List<int> blocksToRemove = new List<int>();
                for (int blockIndex = 0; blockIndex < _blockListProperty.arraySize; blockIndex++)
                {
                    using (new EditorGUILayout.VerticalScope(EditorSkin.customStyles[0]))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField($"<b><color=#FFF>Block: {blockIndex + 1}</color></b>", EditorSkin.label);

                            if (GUILayout.Button("x"))
                            {
                                blocksToRemove.Add(blockIndex);
                            }
                        }

                        EditorGUILayout.Separator();


                        SerializedProperty blockProperty = _blockListProperty.GetArrayElementAtIndex(blockIndex);
                        RenderBlockContent(blockProperty);
                    }
                }

                foreach (int blockIndex in blocksToRemove)
                {
                    _blockListProperty.DeleteArrayElementAtIndex(blockIndex);

                    SerializedObject obj = _blockListProperty.serializedObject;
                    obj.ApplyModifiedProperties();
                    obj.Update();
                }

                if (_blockListProperty.arraySize <= 0) EditorGUILayout.HelpBox("There are no blocks in this session, add one to get a working session.", MessageType.Warning);

                if (GUILayout.Button("Add new block"))
                {
                    _blockListProperty.arraySize++;
                    SerializedProperty blockProperty = _blockListProperty.GetArrayElementAtIndex(_blockListProperty.arraySize - 1);

                    blockProperty.managedReferenceValue = new Block();

                    blockProperty.serializedObject.ApplyModifiedProperties();
                    blockProperty.serializedObject.Update();
                }

                #endregion


                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }



            private void RenderBlockContent(SerializedProperty blockProperty)
            {
                SerializedProperty trialListProperty = blockProperty.FindPropertyRelative("trials");
                SerializedProperty targetRoomProperty = blockProperty.FindPropertyRelative("targetRoom");
                SerializedProperty questionnairePanelListProperty = blockProperty.FindPropertyRelative("blockQuestionnairePanels");


                // ---- Block Data

                #region Block Data

                targetRoomProperty.enumValueIndex = (int) (Enums.Room) EditorGUILayout.EnumPopup("Target Room: ", (Enums.Room) targetRoomProperty.enumValueIndex);

                EditorGUILayout.Separator();

                #endregion
                
                // ---- Block Questions

                #region Block Questions:

                using (new EditorGUILayout.VerticalScope(GUI.skin.customStyles[0]))
                {
                    EditorGUILayout.LabelField($"<b><color=#FFF>Questionnaire Panels: </color></b>", EditorSkin.label);

                    if (questionnairePanelListProperty.arraySize <= 0) EditorGUILayout.HelpBox("There are no questions asked at the end of the trail. Add a question.", MessageType.Warning);

                    List<int> questionsToRemove = new List<int>();
                    for (int questionIndex = 0; questionIndex < questionnairePanelListProperty.arraySize; questionIndex++)
                    {
                        SerializedProperty questionProperty = questionnairePanelListProperty.GetArrayElementAtIndex(questionIndex);

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField($"<b><color=#FFF>Panel {questionIndex + 1}:</color></b>", EditorSkin.label);
                            if (GUILayout.Button("x"))
                            {
                                questionsToRemove.Add(questionIndex);
                            }
                        }



                        EditorGUI.indentLevel += 1;
                        SerializedProperty questionTypeProperty = questionProperty.FindPropertyRelative("questionnairePanelType");

                        EditorGUI.BeginChangeCheck();
                        QuestionnairePanelType_Enum questionnairePanelType = (QuestionnairePanelType_Enum) EditorGUILayout.EnumPopup("Question Type: ", (QuestionnairePanelType_Enum) questionTypeProperty.enumValueIndex);
                        if (EditorGUI.EndChangeCheck())
                        {
                            switch (questionnairePanelType)
                            {
                                case QuestionnairePanelType_Enum.Unselected:
                                {
                                    questionProperty.managedReferenceValue = new QuestionnairePanel();
                                }
                                    break;
                                case QuestionnairePanelType_Enum.OneQuestion_SliderAnswer:
                                {
                                    questionProperty.managedReferenceValue = new QuestionnairePanel_OneSlider();
                                }
                                    break;
                                case QuestionnairePanelType_Enum.TwoQuestion_SliderAnswer:
                                {
                                    questionProperty.managedReferenceValue = new QuestionnairePanel_TwoSliders();
                                }
                                    break;
                            }

                            questionTypeProperty = questionProperty.FindPropertyRelative("questionnairePanelType");
                            questionTypeProperty.enumValueIndex = (int) questionnairePanelType;
                        }

                        switch (questionnairePanelType)
                        {
                            case QuestionnairePanelType_Enum.OneQuestion_SliderAnswer:
                            {
                                SerializedProperty sliderQuestionProperty = questionProperty.FindPropertyRelative("sliderQuestion");
                                {
                                    EditorGUI.indentLevel++;
                                    sliderQuestionProperty.stringValue = EditorGUILayout.TextField("Question: ", sliderQuestionProperty.stringValue);
                                    EditorGUI.indentLevel--;

                                    SerializedProperty questionDecorTypeProperty = questionProperty.FindPropertyRelative("sliderQuestionDecorType");
                                    QuestionDecor_Enum questionDecorType = (QuestionDecor_Enum) EditorGUILayout.EnumPopup("Question Decor Type: ", (QuestionDecor_Enum) questionDecorTypeProperty.enumValueIndex);
                                    questionDecorTypeProperty.enumValueIndex = (int) questionDecorType;
                                    switch (questionDecorType)
                                    {
                                        case QuestionDecor_Enum.Text:
                                        {
                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderQuestionDecorMinimumText");
                                            sliderMinimum.stringValue = EditorGUILayout.TextField("Min: ", sliderMinimum.stringValue);

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderQuestionDecorMaximumText");
                                            sliderMaximum.stringValue = EditorGUILayout.TextField("Max: ", sliderMaximum.stringValue);

                                        }
                                            break;
                                        case QuestionDecor_Enum.Image:
                                        {
                                            EditorGUI.indentLevel++;

                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderQuestionDecorMinimumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMinimum.objectReferenceValue == null) ? "Name does not exist" : sliderMinimum.objectReferenceValue.name)}");
                                                sliderMinimum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMinimum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderQuestionDecorMaximumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMaximum.objectReferenceValue == null) ? "Name does not exist" : sliderMaximum.objectReferenceValue.name)}");
                                                sliderMaximum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMaximum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            EditorGUI.indentLevel--;
                                        }
                                            break;
                                    }
                                }
                            }
                                break;
                            case QuestionnairePanelType_Enum.TwoQuestion_SliderAnswer:
                            {
                                #region Slider Question Two

                                {
                                    EditorGUI.indentLevel++;
                                    SerializedProperty sliderQuestionProperty = questionProperty.FindPropertyRelative("sliderOneQuestion");
                                    sliderQuestionProperty.stringValue = EditorGUILayout.TextField("Question: ", sliderQuestionProperty.stringValue);
                                    EditorGUI.indentLevel--;

                                    SerializedProperty questionDecorTypeProperty = questionProperty.FindPropertyRelative("sliderOneQuestionDecorType");
                                    QuestionDecor_Enum questionDecorType = (QuestionDecor_Enum) EditorGUILayout.EnumPopup("Question Decor Type: ", (QuestionDecor_Enum) questionDecorTypeProperty.enumValueIndex);
                                    questionDecorTypeProperty.enumValueIndex = (int) questionDecorType;
                                    switch (questionDecorType)
                                    {
                                        case QuestionDecor_Enum.Text:
                                        {
                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderOneQuestionDecorMinimumText");
                                            sliderMinimum.stringValue = EditorGUILayout.TextField("Min: ", sliderMinimum.stringValue);

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderOneQuestionDecorMaximumText");
                                            sliderMaximum.stringValue = EditorGUILayout.TextField("Max: ", sliderMaximum.stringValue);

                                        }
                                            break;
                                        case QuestionDecor_Enum.Image:
                                        {
                                            EditorGUI.indentLevel++;

                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderOneQuestionDecorMinimumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMinimum.objectReferenceValue == null) ? "Name does not exist" : sliderMinimum.objectReferenceValue.name)}");
                                                sliderMinimum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMinimum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderOneQuestionDecorMaximumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMaximum.objectReferenceValue == null) ? "Name does not exist" : sliderMaximum.objectReferenceValue.name)}");
                                                sliderMaximum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMaximum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            EditorGUI.indentLevel--;
                                        }
                                            break;
                                    }
                                }

                                #endregion

                                EditorGUILayout.Separator();

                                #region Slider Question Two

                                {
                                    EditorGUI.indentLevel++;
                                    SerializedProperty sliderQuestionProperty = questionProperty.FindPropertyRelative("sliderTwoQuestion");
                                    sliderQuestionProperty.stringValue = EditorGUILayout.TextField("Question: ", sliderQuestionProperty.stringValue);
                                    EditorGUI.indentLevel--;

                                    SerializedProperty questionDecorTypeProperty = questionProperty.FindPropertyRelative("sliderTwoQuestionDecorType");
                                    QuestionDecor_Enum questionDecorType = (QuestionDecor_Enum) EditorGUILayout.EnumPopup("Question Decor Type: ", (QuestionDecor_Enum) questionDecorTypeProperty.enumValueIndex);
                                    questionDecorTypeProperty.enumValueIndex = (int) questionDecorType;
                                    switch (questionDecorType)
                                    {
                                        case QuestionDecor_Enum.Text:
                                        {
                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderTwoQuestionDecorMinimumText");
                                            sliderMinimum.stringValue = EditorGUILayout.TextField("Min: ", sliderMinimum.stringValue);

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderTwoQuestionDecorMaximumText");
                                            sliderMaximum.stringValue = EditorGUILayout.TextField("Max: ", sliderMaximum.stringValue);

                                        }
                                            break;
                                        case QuestionDecor_Enum.Image:
                                        {
                                            EditorGUI.indentLevel++;

                                            SerializedProperty sliderMinimum = questionProperty.FindPropertyRelative("sliderTwoQuestionDecorMinimumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMinimum.objectReferenceValue == null) ? "Name does not exist" : sliderMinimum.objectReferenceValue.name)}");
                                                sliderMinimum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMinimum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            SerializedProperty sliderMaximum = questionProperty.FindPropertyRelative("sliderTwoQuestionDecorMaximumSprite");
                                            using (new EditorGUILayout.HorizontalScope(_editorSkin.customStyles[1]))
                                            {
                                                EditorGUILayout.LabelField($"Object: {((sliderMaximum.objectReferenceValue == null) ? "Name does not exist" : sliderMaximum.objectReferenceValue.name)}");
                                                sliderMaximum.objectReferenceValue = EditorGUILayout.ObjectField(sliderMaximum.objectReferenceValue, typeof(Texture2D), false);
                                            }

                                            EditorGUI.indentLevel--;
                                        }
                                            break;
                                    }
                                }

                                #endregion
                            }
                                break;
                        }

                        EditorGUI.indentLevel -= 1;
                        EditorGUILayout.Space();
                    }

                    foreach (int questionIndex in questionsToRemove)
                    {
                        questionnairePanelListProperty.DeleteArrayElementAtIndex(questionIndex);

                        SerializedObject obj = _blockListProperty.serializedObject;
                        obj.ApplyModifiedProperties();
                        obj.Update();
                    }

                    if (GUILayout.Button("Add new 'Question'"))
                    {
                        questionnairePanelListProperty.arraySize++;
                        SerializedProperty questionProperty = questionnairePanelListProperty.GetArrayElementAtIndex(questionnairePanelListProperty.arraySize - 1);

                        questionProperty.managedReferenceValue = new QuestionnairePanel();

                        questionProperty.serializedObject.ApplyModifiedProperties();
                        questionProperty.serializedObject.Update();
                    }
                }

                #endregion

                // ---- Block Trials

                #region Render Block Trials

                using (new EditorGUILayout.VerticalScope(EditorSkin.customStyles[0]))
                {
                    int trialCount = trialListProperty.arraySize;
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUILayout.LabelField($"<b><color=#FFF>Trials: </color></b>", EditorSkin.label);
                        trialCount = EditorGUILayout.IntField(trialCount);
                    }

                    if (trialCount != trialListProperty.arraySize)
                    {
                        while (trialCount > trialListProperty.arraySize)
                        {
                            trialListProperty.arraySize++;
                            SerializedProperty trialProperty = trialListProperty.GetArrayElementAtIndex(trialListProperty.arraySize - 1);

                            trialProperty.managedReferenceValue = new Trial();

                            trialProperty.serializedObject.ApplyModifiedProperties();
                            trialProperty.serializedObject.Update();
                        }

                        while (trialCount >= 0 && trialCount < trialListProperty.arraySize)
                        {
                            trialListProperty.DeleteArrayElementAtIndex(trialCount);
                        }
                    }

                    List<int> trialsToRemove = new List<int>();
                    for (int trialIndex = 0; trialIndex < trialListProperty.arraySize; trialIndex++)
                    {
                        EditorGUILayout.Separator();

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField($"<b><color=#FFF>Trial {trialIndex + 1}:</color></b>", EditorSkin.label);
                            if (GUILayout.Button("x"))
                            {
                                trialsToRemove.Add(trialIndex);
                            }
                        }

                        EditorGUILayout.Space(5);

                        SerializedProperty trialProperty = trialListProperty.GetArrayElementAtIndex(trialIndex);

                        SerializedProperty roomA_NPCAvatarProperty = trialProperty.FindPropertyRelative("roomA_NPCAvatar");
                        SerializedProperty roomB_NPCAvatarProperty = trialProperty.FindPropertyRelative("roomB_NPCAvatar");
                        SerializedProperty heldObjectProperty = trialProperty.FindPropertyRelative("heldObject");

                        if (roomA_NPCAvatarProperty.objectReferenceValue == null || roomB_NPCAvatarProperty.objectReferenceValue == null || heldObjectProperty.objectReferenceValue == null)
                        {
                            EditorGUILayout.HelpBox("These fields cannot be empty, assign a Prefab.", MessageType.Error);
                            EditorGUILayout.Space(5);
                        }

                        EditorGUI.indentLevel += 1;
                        roomA_NPCAvatarProperty.objectReferenceValue = EditorGUILayout.ObjectField("Room A: NPC Avatar", roomA_NPCAvatarProperty.objectReferenceValue, typeof(GameObject), false);
                        roomB_NPCAvatarProperty.objectReferenceValue = EditorGUILayout.ObjectField("Room B: NPC Avatar", roomB_NPCAvatarProperty.objectReferenceValue, typeof(GameObject), false);

                        heldObjectProperty.objectReferenceValue = EditorGUILayout.ObjectField("Held Item", heldObjectProperty.objectReferenceValue, typeof(GameObject), false);


                        EditorGUI.indentLevel -= 1;
                        EditorGUILayout.Space();
                    }

                    foreach (int trialIndex in trialsToRemove)
                    {
                        trialListProperty.DeleteArrayElementAtIndex(trialIndex);

                        SerializedObject obj = _blockListProperty.serializedObject;
                        obj.ApplyModifiedProperties();
                        obj.Update();
                    }


                    if (trialListProperty.arraySize <= 0) EditorGUILayout.HelpBox("There are no trials in this block.", MessageType.Warning);

                    if (GUILayout.Button("Add new Trial"))
                    {
                        trialListProperty.arraySize++;
                        SerializedProperty trialProperty = trialListProperty.GetArrayElementAtIndex(trialListProperty.arraySize - 1);

                        trialProperty.managedReferenceValue = new Trial();

                        trialProperty.serializedObject.ApplyModifiedProperties();
                        trialProperty.serializedObject.Update();
                    }
                }
                
                #endregion
            }
        }
    }
}

#endif