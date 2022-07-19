using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    namespace DataCollection
    {
        public class TransformSampleData
        {
            // Time of data sample since program start:
            public float time;

            // Stored in the format of Vector3:
            public float[] cameraOffsetPosition, playerHeadPosition, playerLeftHandPosition, playerRightHandPosition;

            // Stored in the format of Quaternion:
            public float[] cameraOffsetRotation, playerHeadRotation, playerLeftHandRotation, playerRightHandRotation;


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
        }
    }
}