#if PLATFORM_STANDALONE_WIN
using LSL;
#endif

namespace _LSL
{
    public class LSLOutput_MarkerStream : LSLOutput<string>
    {
#if PLATFORM_STANDALONE_WIN
        private void Awake()
        {
            StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_string);
            XMLElement channels = streamInfo.desc().append_child("channels");
            channels.append_child("channel").append_child_value("label", "Marker");

            _outlet = new StreamOutlet(streamInfo);
            _currentSample = new string[1];
        }

        public void PublishMarkerToNetwork(in string marker)
        {
            _currentSample = new[] { marker };
            PushOutput(_outlet, _currentSample);
        }
#endif
    }
}