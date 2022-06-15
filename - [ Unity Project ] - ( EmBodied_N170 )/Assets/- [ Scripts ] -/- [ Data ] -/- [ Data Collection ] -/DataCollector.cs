using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DataCollection
{
    public class DataCollector : MonoBehaviour
    {
        public readonly DataContainer dataContainer = new DataContainer();

        
        
        
        
        public void WriteData()
        {
            string dateTime = $"{DateTime.Now:U}";
            
            string directory = $"{Application.persistentDataPath}/Data/";
            string filename = $"DataContainer - {dateTime.Replace(':', '-')}";

            WriteToJSON(directory, filename);
        }

        private void WriteToJSON(in string directory, in string filename)
        {
            string jsonData = JsonConvert.SerializeObject(dataContainer, Formatting.Indented);
            
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            try
            {
                File.WriteAllText($"{directory}{filename}", jsonData);
                Debug.Log($"Entered State: <color=#4F4>Data Written To:</color> {directory}{filename}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void WriteToCSV()
        {
            
        }
    }
}