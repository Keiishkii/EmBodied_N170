using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Questionnaire;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

namespace Data
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
        
        private QuestionType _questionType;
        private enum QuestionType
        {
            ChooseAPanelType,
            OneQuestionWithSliderAnswer,
            TwoQuestionsWithSliderAnswer 
        }

        private SerializedProperty _approachDistanceProperty;
        private SerializedProperty _blockListProperty;
        
        


        
        private void OnEnable()
        {
            _approachDistanceProperty = serializedObject.FindProperty("approachDistance");
            _blockListProperty = serializedObject.FindProperty("blocks");
        }

        
        
        
        
        public override void OnInspectorGUI()
        {
            GUI.skin = EditorSkin;
            
            
            GUILayout.BeginVertical(EditorSkin.customStyles[0]);
            _approachDistanceProperty.floatValue = EditorGUILayout.FloatField("Approach Distance: ", _approachDistanceProperty.floatValue);
            GUILayout.EndVertical();
            
            
            
            List<int> blocksToRemove = new List<int>();
            for (int blockIndex = 0; blockIndex < _blockListProperty.arraySize; blockIndex++)
            {
                EditorGUILayout.BeginVertical(GUI.skin.customStyles[0]);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"<b><color=#FFF>Block: {blockIndex + 1}</color></b>", EditorSkin.label);

                    if (GUILayout.Button("x"))
                    {
                        blocksToRemove.Add(blockIndex);
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                
                EditorGUILayout.Separator();

                SerializedProperty blockProperty = _blockListProperty.GetArrayElementAtIndex(blockIndex);
                RenderBlock(blockProperty);
                
                EditorGUILayout.EndVertical();
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

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }



        private void RenderBlock(SerializedProperty blockProperty)
        {
            
            SerializedProperty trialListProperty = blockProperty.FindPropertyRelative("trials");
            SerializedProperty questionListProperty = blockProperty.FindPropertyRelative("blockQuestions");
            
            
            
            GUILayout.BeginVertical(GUI.skin.customStyles[0]);
            {
                EditorGUILayout.LabelField($"<b><color=#FFF>Questions: </color></b>", EditorSkin.label);
                
                if (questionListProperty.arraySize <= 0) EditorGUILayout.HelpBox("There are no questions asked at the end of the trail. Add a question.", MessageType.Warning);

                List<int> questionsToRemove = new List<int>();
                for (int questionIndex = 0; questionIndex < questionListProperty.arraySize; questionIndex++)
                {
                    SerializedProperty questionProperty = questionListProperty.GetArrayElementAtIndex(questionIndex);
                    
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"<b><color=#FFF>Question: {questionIndex + 1}</color></b>", EditorSkin.label);
                        if (GUILayout.Button("x"))
                        {
                            questionsToRemove.Add(questionIndex);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    
                    
                    EditorGUI.indentLevel += 1;
                    SerializedProperty questionTypeProperty = questionProperty.FindPropertyRelative("questionType");

                    EditorGUI.BeginChangeCheck();
                    QuestionType_Enum questionType = (QuestionType_Enum) EditorGUILayout.EnumPopup("Question Type: ",  (QuestionType_Enum) questionTypeProperty.enumValueIndex);
                    if (EditorGUI.EndChangeCheck()) 
                    {
                        switch (questionType)
                        {
                            case QuestionType_Enum.Unselected:
                            {
                                questionProperty.managedReferenceValue = new BlockQuestion();
                            } break;
                            case QuestionType_Enum.OneQuestion_SliderAnswer:
                            {
                                questionProperty.managedReferenceValue = new BlockQuestion_OneQuestionSliderAnswer();
                            } break;
                            case QuestionType_Enum.TwoQuestion_SliderAnswer:
                            {
                                questionProperty.managedReferenceValue = new BlockQuestion_TwoQuestionSliderAnswer();
                            } break;
                        }
                        
                        questionTypeProperty = questionProperty.FindPropertyRelative("questionType");
                        questionTypeProperty.enumValueIndex = (int) questionType;
                    }
                    
                    switch (questionType)
                    {
                        case QuestionType_Enum.OneQuestion_SliderAnswer:
                        {
                            SerializedProperty questionOneProperty = questionProperty.FindPropertyRelative("questionOne");
                            questionOneProperty.stringValue = EditorGUILayout.TextField("Question One: ",  questionOneProperty.stringValue);
                            
                        } break;
                        case QuestionType_Enum.TwoQuestion_SliderAnswer:
                        {
                            SerializedProperty questionOneProperty = questionProperty.FindPropertyRelative("questionOne");
                            questionOneProperty.stringValue = EditorGUILayout.TextField("Question One: ",  questionOneProperty.stringValue);
                            
                            SerializedProperty questionTwoProperty = questionProperty.FindPropertyRelative("questionTwo");
                            questionTwoProperty.stringValue = EditorGUILayout.TextField("Question Two: ",  questionTwoProperty.stringValue);
                            
                        } break;
                    }
                    
                    EditorGUI.indentLevel -= 1;
                    EditorGUILayout.Space();
                }

                foreach (int questionIndex in questionsToRemove)
                {
                    questionListProperty.DeleteArrayElementAtIndex(questionIndex);

                    SerializedObject obj = _blockListProperty.serializedObject;
                    obj.ApplyModifiedProperties();
                    obj.Update();
                }
                
                if (GUILayout.Button("Add new 'Question'"))
                {
                    questionListProperty.arraySize++;
                    SerializedProperty questionProperty = questionListProperty.GetArrayElementAtIndex(questionListProperty.arraySize - 1);

                    questionProperty.managedReferenceValue = new BlockQuestion();

                    questionProperty.serializedObject.ApplyModifiedProperties();
                    questionProperty.serializedObject.Update();
                }
            }
            GUILayout.EndVertical();

            
            
            
            GUILayout.BeginVertical(GUI.skin.customStyles[0]);
            {
                EditorGUILayout.LabelField($"<b><color=#FFF>Trials: </color></b>", EditorSkin.label);
                
                int value = trialListProperty.arraySize;
                int trialCount = EditorGUILayout.IntField("Trial Count: ", value);
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

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"<b><color=#FFF>Trial: {trialIndex + 1}</color></b>", EditorSkin.label);
                        if (GUILayout.Button("x"))
                        {
                            trialsToRemove.Add(trialIndex);
                        }
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Space(5);

                    SerializedProperty trialProperty = trialListProperty.GetArrayElementAtIndex(trialIndex); 

                    SerializedProperty roomA_NPCAvatarProperty = trialProperty.FindPropertyRelative("roomA_NPCAvatar");
                    SerializedProperty roomB_NPCAvatarProperty = trialProperty.FindPropertyRelative("roomB_NPCAvatar");
                    SerializedProperty heldObjectProperty = trialProperty.FindPropertyRelative("heldObject");

                    EditorGUI.indentLevel += 1;
                    roomA_NPCAvatarProperty.objectReferenceValue = EditorGUILayout.ObjectField("Room A: NPC Avatar", roomA_NPCAvatarProperty.objectReferenceValue, typeof(GameObject), false);
                    roomB_NPCAvatarProperty.objectReferenceValue = EditorGUILayout.ObjectField("Room B: NPC Avatar", roomB_NPCAvatarProperty.objectReferenceValue, typeof(GameObject), false);

                    EditorGUILayout.Space();

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
            GUILayout.EndVertical();
            
            
            /*

            EditorGUILayout.Separator();
            
            List<int> questionsToRemove = new List<int>();
            foreach (var (blockQuestion, questionIndex) in block.blockQuestions.Select((value, i) => (value, i)))
            {
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField($"Question: {questionIndex + 1}");
                    if (GUILayout.Button("x"))
                        questionsToRemove.Add(questionIndex);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel += 1;
                switch (blockQuestion)
                {
                    case BlockQuestion_OneQuestionSliderAnswer question:
                    {
                        question.questionOne = EditorGUILayout.TextArea(question.questionOne);
                    } break;
                    case BlockQuestion_TwoQuestionSliderAnswer question:
                    {
                        question.questionOne = EditorGUILayout.TextArea(question.questionOne);
                        question.questionTwo = EditorGUILayout.TextArea(question.questionTwo);
                    } break;
                }

                EditorGUI.indentLevel -= 1;
            }

            foreach (int questionIndex in questionsToRemove)
                block.blockQuestions.RemoveAt(questionIndex);

            
            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            _questionType = (QuestionType) EditorGUILayout.EnumPopup(_questionType);
            if (GUILayout.Button("Add new question to block"))
            {
                switch (_questionType)
                {
                    case QuestionType.ChooseAPanelType:
                    {

                    } break;
                    case QuestionType.OneQuestionWithSliderAnswer:
                    {
                        block.blockQuestions.Add(new BlockQuestion_OneQuestionSliderAnswer());
                    } break;
                    case QuestionType.TwoQuestionsWithSliderAnswer:
                    {
                        block.blockQuestions.Add(new BlockQuestion_TwoQuestionSliderAnswer());
                    } break;
                }
            }

            EditorGUILayout.EndHorizontal();
            

            if (_blockListProperty.arraySize <= 0)
                EditorGUILayout.HelpBox("There are no questions asked at the end of the trail. Add a question.", MessageType.Warning);
            
            */
        }
    }
}