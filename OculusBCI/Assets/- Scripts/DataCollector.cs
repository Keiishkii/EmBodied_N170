using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace DataCollection
{
    public class DataCollector : MonoBehaviour
    {
        private string _directory;
        private string _fileName;
        
        private readonly List<AnswerContainer> _answerContainers = new List<AnswerContainer>();
        private readonly List<DataContainer> _dataContainers = new List<DataContainer>();


        private void Awake()
        {
            _directory = $"{Application.persistentDataPath}/Data/";
            _fileName = $"Recorded Data - {DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv";
        }

        public void AddQuestionResults(string question, string answer)
        {
            _answerContainers.Add(new AnswerContainer()
            {
                question = question, 
                answer = answer
            });
        }
    
        public void AddDataToContainer(string marker, Transform player)
        {
            _dataContainers.Add(new DataContainer()
            {
                marker = marker, 
                time = $"{DateTime.Now:HH-mm-ss.fff}",
                playerHeadPosition = player.position,
                playerHeadRotation = player.rotation
            });
        }

        public void PublishData()
        {
            string csvString = "";

            
            WriteAnswerContainerToCSV(ref csvString);
            csvString += "\n";
            WriteDataContainerToCSV(ref csvString);
            
            
            WriteToCSVFile(csvString);
        }

        private void WriteAnswerContainerToCSV(ref string csvContent)
        {
            string questionData = "", answerData = "";
            foreach (var container in _answerContainers)
            {
                questionData += $"{container.question},";
                answerData += $"{container.answer},";
            }

            csvContent += $"Questions:,{questionData}\n";
            csvContent += $"Answers:,{answerData}\n";
        }
        
        private void WriteDataContainerToCSV(ref string csvContent)
        {
            string dataTitles = ",Marker,Time,Position: x,Position: y,Position: z,Rotation: x,Rotation: y,Rotation: z,Rotation: w";

            int index = 0;
            string dataContent = "";
            foreach (var container in _dataContainers)
            {
                dataContent += $"{index},{container.marker},{container.time}," +
                               $"{container.playerHeadPosition.x},{container.playerHeadPosition.y},{container.playerHeadPosition.z}, " +
                               $"{container.playerHeadRotation.x},{container.playerHeadRotation.y},{container.playerHeadRotation.z},{container.playerHeadRotation.w}\n";
                
                index++;
            }

            csvContent += $"{dataTitles}\n";
            csvContent += $"{dataContent}\n";
        }

        private void WriteToCSVFile(string csvContent)
        {
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }

            File.WriteAllText($"{_directory}{_fileName}", csvContent);
        }
    }
}
