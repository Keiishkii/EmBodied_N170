#if (PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
using LSL;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.InputAction;
#endif

namespace _LSL
{
    public class LSLOutput_KeyboardMarkerStream : LSLOutput<string>
    {
#if (PLATFORM_STANDALONE_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        [SerializeField] private InputActionReference keyboardInput; 

        private void Awake()
        {
            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_string);
            XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "Marker");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new string[1];

            keyboardInput.action.performed += KeyboardPress;
        }

        private void OnDestroy()
        {
            keyboardInput.action.performed -= KeyboardPress;
        }


        private void KeyboardPress(CallbackContext ctx)
        {
            string key = "key";
            Debug.Log($"Key: {key}");
            PublishMarkerToNetwork(key);
        }

        public void PublishMarkerToNetwork(in string marker)
        {
            _currentSample = new[] { marker };
            PushOutput(_outlet, _currentSample);
        }
#endif
    }
}