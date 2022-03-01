using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLMarkerOutputStream : MonoBehaviour
{
    [SerializeField] private string _streamName = "A";
    [SerializeField] private string _streamType = "A";
    
    private StreamOutlet _outlet;
    private string[] _marker;
    
    
    
    
    void Start()
    {
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 1, 0, channel_format_t.cf_string);
        XMLElement channels = streamInfo.desc().append_child("channels");
        channels.append_child("channel").append_child_value("label", "Marker");
        
        _outlet = new StreamOutlet(streamInfo);
        _marker = new string[1];
    }

    
    
    public void WriteMarker(string marker)
    {
        _marker[0] = marker;
        _outlet.push_sample(_marker);
    }
}