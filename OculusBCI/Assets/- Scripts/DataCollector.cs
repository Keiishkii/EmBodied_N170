using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TestQuestions;
using UnityEngine;

namespace DataCollection
{
    public class DataCollector : MonoBehaviour
    {
        private string _directory;
        private string _fileName;
        
        private readonly List<AnswersContainer> _answerContainers = new List<AnswersContainer>();
        private readonly List<DataContainer> _dataContainers = new List<DataContainer>();


        private void Awake()
        {
            _directory = $"{Application.persistentDataPath}/Data/";
            _fileName = $"Recorded Data - {DateTime.Now:yyyy-dd-M--HH-mm-ss}.csv";
        }

        public void AddNewAnswerGroup()
        {
            _answerContainers.Add(new AnswersContainer());
        }

        public void AddNewAnswer(string answerValue)
        {
            Answer answer = new Answer()
            {
                answer = answerValue
            };
            
            _answerContainers[_answerContainers.Count - 1].answers.Add(answer);
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

        public void PublishData(List<TestQuestion> testQuestions)
        {
            string csvString = "";

            csvString += "\n";
            WriteDataContainerToCSVString(ref csvString, ref testQuestions);
            
            WriteToCSVFile(csvString);
        }
        
        private void WriteDataContainerToCSVString(ref string csvContent, ref List<TestQuestions.TestQuestion> testQuestions)
        {
            string dataTitles = ",Marker,Time,Position: x,Position: y,Position: z,Rotation: x,Rotation: y,Rotation: z,Rotation: w,";

            string questionHeaders = "";
            foreach (var testQuestion in testQuestions)
            {
                switch (testQuestion)
                {
                    case TestQuestion_1_Slider question:
                    {
                        questionHeaders += $"{question.questionGroup.question},";
                    } break;
                    case TestQuestion_2_Slider question:
                    {
                        questionHeaders += $"{question.questionGroupOne.question},";
                        questionHeaders += $"{question.questionGroupTwo.question},";
                    } break;
                }
            }

            dataTitles += questionHeaders;
            

            int index = 0;
            int answerIndex = 0;
            string dataContent = "";
            foreach (var container in _dataContainers)
            {
                dataContent += $"{index},{container.marker},{container.time}," +
                               $"{container.playerHeadPosition.x},{container.playerHeadPosition.y},{container.playerHeadPosition.z}, " +
                               $"{container.playerHeadRotation.x},{container.playerHeadRotation.y},{container.playerHeadRotation.z},{container.playerHeadRotation.w},";

                if (container.marker == $"{GameState_Enum.PLAYER_RESULTS_COLLECTED}")
                {
                    foreach (var answer in _answerContainers[answerIndex].answers)
                    {
                        dataContent += $"{answer.answer},";
                    }

                    answerIndex++;
                }

                dataContent += "\n";
                
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
