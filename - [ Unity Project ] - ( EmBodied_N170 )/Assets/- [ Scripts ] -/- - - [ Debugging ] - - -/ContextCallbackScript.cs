using System;
using System.IO;
using UnityEngine;

public class ContextCallbackScript : MonoBehaviour
{
    [ContextMenu("Write Test File")]
    private void WriteTestFile()
    {
        TestData data = new TestData();

        string directory = $"{Application.persistentDataPath}/Test Data/";
        string filename = $"testData.json";
        
        string fileContents = JsonUtility.ToJson(data, true);

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        File.WriteAllText($"{directory}{filename}", fileContents);
        
        Debug.Log($"File written to: {directory}{filename}");
    }
}

[Serializable]
public class TestData
{
    public int number = 0;
    public int index = 5;
    public string name = "Paul";
    public string message = "Hello";
}