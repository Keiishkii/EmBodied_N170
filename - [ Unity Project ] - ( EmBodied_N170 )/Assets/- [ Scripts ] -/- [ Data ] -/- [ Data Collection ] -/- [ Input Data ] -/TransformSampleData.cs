using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// Class for storing a single frames worth of transform data for all useful transforms within the experiment.
        /// Useful to look at what the participant was doing at a given time and to compare against any potential discrepancies within the data.
        /// </summary>
        public class TransformSampleData
        {
            // Time of data sample since program start:
            public float time;

            // Stored in the format of Vector3:
            public float[] cameraOffsetPosition, playerHeadPosition, playerLeftHandPosition, playerRightHandPosition;

            // Stored in the format of Quaternion:
            public float[] cameraOffsetRotation, playerHeadRotation, playerLeftHandRotation, playerRightHandRotation;


            // Converts the Camera transforms Unity Vector3 and Quaternion to an array of floats, for saving as JSON using Newtonsoft. 
            public Transform SetCameraOffsetTransform
            {
                set
                {
                    cameraOffsetPosition = new float[3]
                    {
                        value.position.x,
                        value.position.y,
                        value.position.z
                    };

                    cameraOffsetRotation = new float[4]
                    {
                        value.rotation.x,
                        value.rotation.y,
                        value.rotation.z,
                        value.rotation.w
                    };
                }
            }

            // Converts the Head transforms Unity Vector3 and Quaternion to an array of floats, for saving as JSON using Newtonsoft. 
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

            // Converts the Left Hand transforms Unity Vector3 and Quaternion to an array of floats, for saving as JSON using Newtonsoft. 
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

            // Converts the Right Hand transforms Unity Vector3 and Quaternion to an array of floats, for saving as JSON using Newtonsoft. 
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
        }
    }
}