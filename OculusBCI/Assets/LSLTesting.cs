using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

public class LSLTesting : MonoBehaviour
{
    [SerializeField] private Transform _orbitingTransform;
    
    private string StreamName = "Headset Position";
    private string StreamType = "Position Tracker";

    private StreamOutlet outlet;
    
    
    
    void Awake()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, channel_format_t.cf_float32);
        XMLElement channels = streamInfo.desc().append_child("channels");
        
        channels.append_child("channel").append_child_value("label", "X");
        channels.append_child("channel").append_child_value("label", "Y");
        channels.append_child("channel").append_child_value("label", "Z");
        
        outlet = new StreamOutlet(streamInfo);
    }

    void FixedUpdate()
    {
        Vector3 headsetPosition = _orbitingTransform.position;
        float[] positionData = {headsetPosition.x, headsetPosition.y, headsetPosition.z};
        
        outlet.push_sample(positionData);
    }
}
