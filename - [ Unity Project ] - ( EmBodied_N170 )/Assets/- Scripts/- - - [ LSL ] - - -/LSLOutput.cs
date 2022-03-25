using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLOutput : MonoBehaviour
{
    [SerializeField] private string _streamName = "A";
    [SerializeField] private string _streamType = "A";
    
    private StreamOutlet _outlet;
    private float[] _currentSample;

    
    
    
    
    void Start()
    {
        float sampleRate = (1 / Time.fixedDeltaTime);
        
        StreamInfo streamInfo = new StreamInfo(_streamName, _streamType, 3, sampleRate, channel_format_t.cf_float32);
        XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "X");
        chans.append_child("channel").append_child_value("label", "Y");
        chans.append_child("channel").append_child_value("label", "Z");
        
        _outlet = new StreamOutlet(streamInfo);
        _currentSample = new float[3];
    }

    void FixedUpdate()
    {
        Vector3 pos = gameObject.transform.position;
        _currentSample[0] = pos.x;
        _currentSample[1] = pos.y;
        _currentSample[2] = pos.z;
        
        _outlet.push_sample(_currentSample);
    }
}