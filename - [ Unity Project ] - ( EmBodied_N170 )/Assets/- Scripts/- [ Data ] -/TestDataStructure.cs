using System;
using System.Collections.Generic;
using System.IO;
using TestQuestions;
using UnityEngine;

[Serializable]
public class TestDataStructure
{
    private static string _dataPath = "/- Data/";
    private static string _defaultName = "default_structure.json";
    public static TestDataStructure DefaultFile
    {
        get
        {
            TestDataStructure defaultFile = null;

            string fullDirectory = $"{Application.dataPath}{_dataPath}";
            if (!Directory.Exists(fullDirectory))
                Directory.CreateDirectory(fullDirectory);

            string fullPath = $"{fullDirectory}{_defaultName}";
            if (File.Exists(fullPath))
            {
                string fileContent = File.ReadAllText(fullPath);
                defaultFile = JsonUtility.FromJson<TestDataStructure>(fileContent);
            }
            else
            {
                defaultFile = new TestDataStructure
                {
                    testSegments = new List<TestSegment>
                    {
                        new TestSegment()
                    }
                };

                string jsonContent = JsonUtility.ToJson(defaultFile, true);
                File.WriteAllText(fullPath, jsonContent);
            }

            return defaultFile;
        }
    }

    
    
    public float distanceToTriggerNPCLookUp = 2.0f;
    public float distanceToTriggerPlayerLeave = 3.5f;
    public float distanceToTriggerReturnedToCentre = 1f;
    
    public List<TestSegment> testSegments;
    
    public List<string> testQuestionJSONList;
    
    [NonSerialized]
    public readonly List<TestQuestion> testQuestionList = new List<TestQuestion>();
    
    
    public static TestDataStructure LoadJSONData(TextAsset jsonFile)
    {
        TestDataStructure testDataStructure = null;
        
        if (jsonFile != null)
        {
            testDataStructure = JsonUtility.FromJson<TestDataStructure>(jsonFile.text);

            foreach (string testQuestion in testDataStructure.testQuestionJSONList)
            {
                TestQuestion baseQuestion = JsonUtility.FromJson<TestQuestions.TestQuestion>(testQuestion);
                switch (baseQuestion.questionType)
                {
                    case TestQuestionType.OneQuestion_SliderAnswer:
                    {
                        TestQuestion_1_Slider question = JsonUtility.FromJson<TestQuestion_1_Slider>(testQuestion);
                        testDataStructure.testQuestionList.Add(question);
                    } break;
                    case TestQuestionType.TwoQuestions_SliderAnswer:
                    {
                        TestQuestion_2_Slider question = JsonUtility.FromJson<TestQuestion_2_Slider>(testQuestion);
                        testDataStructure.testQuestionList.Add(question);
                    } break;
                }
            }
        }
        else
        {
            testDataStructure = DefaultFile;
        }

        return testDataStructure;
    }
}
