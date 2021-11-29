using System;
using System.Collections.Generic;
using System.IO;
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

    public float distanceToTriggerLookUp = 2.5f;
    public List<TestSegment> testSegments;
}
