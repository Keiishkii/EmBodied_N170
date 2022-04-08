using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Questionnaire;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering;
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
                        blocksToRemove.Add(blockIndex);
                }
                EditorGUILayout.EndHorizontal();
                
                
                EditorGUILayout.Separator();

                SerializedProperty blockProperty = _blockListProperty.GetArrayElementAtIndex(blockIndex);
                RenderBlock(blockProperty, blockIndex);
                
                EditorGUILayout.EndVertical();
            }

            foreach (int blockIndex in blocksToRemove)
            {
                _blockListProperty.DeleteArrayElementAtIndex(blockIndex);
                
                SerializedObject obj = _blockListProperty.serializedObject;
                obj.ApplyModifiedProperties();
                obj.Update();
            }
            
             
            
            if (GUILayout.Button("Add new block"))
            {
                _blockListProperty.arraySize++;
                SerializedProperty blockProperty = _blockListProperty.GetArrayElementAtIndex(_blockListProperty.arraySize - 1);
                
                blockProperty.managedReferenceValue = new Block();

                SerializedObject obj = blockProperty.serializedObject;
                obj.ApplyModifiedProperties();
                obj.Update();
            }

            if (_blockListProperty.arraySize <= 0)
                EditorGUILayout.HelpBox("There are no blocks ins the trial, add one to get a working session.", MessageType.Warning);
            
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }



        private void RenderBlock(SerializedProperty blockProperty, int blockIndex)
        {
            
            SerializedProperty trialListProperty = blockProperty.FindPropertyRelative("trials");
            SerializedProperty blockQuestionListProperty = blockProperty.FindPropertyRelative("blockQuestions");
            
            //GUILayout.BeginVertical(GUI.skin.customStyles[0]);

            int value = trialListProperty.arraySize;
            Debug.Log(value);
            
            
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
            
            /*
            foreach (var (trial, trialIndex) in block.trials.Select((value, i) => (value, i)))
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField($"<b><color=#FFF>Trial: {trialIndex + 1}</color></b>", EditorSkin.label);
                
                //EditorGUI.indentLevel += 1;
                    trial.NPCAvatar = (GameObject) EditorGUILayout.ObjectField("NPC Avatar: ", trial.NPCAvatar, typeof(GameObject), true);
                    trial.heldObject = (GameObject) EditorGUILayout.ObjectField("Held Object: ", trial.heldObject, typeof(GameObject), true);
                //EditorGUI.indentLevel -= 1;
            }
            

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
            
            //GUILayout.EndVertical();
            */
        }
    }
}