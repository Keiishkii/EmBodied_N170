#if (PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
using LSL;
#endif

using UnityEngine;

namespace _LSL
{
    public class LSLOutput_PositionStream : LSLOutput<float>
    {
#if (PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        private float _sampleRate;
        public Transform positionStreamTarget;
        
        

        private void Awake()
        {
            _sampleRate = (1.0f / Time.fixedDeltaTime);
            
            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 3, _sampleRate, channel_format_t.cf_float32);
            XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "X");
            channels.append_child("channel").append_child_value("label", "Y");
            channels.append_child("channel").append_child_value("label", "Z");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new float[3];
        }

        private void FixedUpdate()
        {
            if (!positionStreamTarget) return;
            Vector3 position = positionStreamTarget.position;

            _currentSample = new[]
            {
                position.x,
                position.y,
                position.z
            };

            PushOutput(_outlet, _currentSample);
        }
#endif
    }
}