using System;
using Unity.Mathematics;
using UnityEngine;

namespace DataCollection
{
    [Serializable]
    public class DataCollectionEvent_RecordMarker : DataCollectionEvent_Interface
    {
        public float timeSinceProgramStart;
        public string currentState;
        
        public float[] playerHeadPosition, playerLeftHandPosition, playerRightHandPosition;
        public float[] playerHeadRotation, playerLeftHandRotation, playerRightHandRotation;
        public Transform SetHeadTransform
        {
            set
            {
                playerHeadPosition = new float[3]
                {
                    value.position.x,
                    value.position.y,
                    value.position.z
                };

                playerHeadRotation = new float[4]
                {
                    value.rotation.x,
                    value.rotation.y,
                    value.rotation.z,
                    value.rotation.w
                };
            }
        }
        public Transform SetLeftHandTransform
        {
            set
            {
                playerLeftHandPosition = new float[3]
                {
                    value.position.x,
                    value.position.y,
                    value.position.z
                };

                playerLeftHandRotation = new float[4]
                {
                    value.rotation.x,
                    value.rotation.y,
                    value.rotation.z,
                    value.rotation.w
                };
            }
        }
        public Transform SetRightHandTransform
        {
            set
            {
                playerRightHandPosition = new float[3]
                {
                    value.position.x,
                    value.position.y,
                    value.position.z
                };

                playerRightHandRotation = new float[4]
                {
                    value.rotation.x,
                    value.rotation.y,
                    value.rotation.z,
                    value.rotation.w
                };
            }
        }
        
        
        public DataCollectionEvent_RecordMarker()
        {
            type = nameof(DataCollectionEvent_RecordMarker);
        }
    }
}