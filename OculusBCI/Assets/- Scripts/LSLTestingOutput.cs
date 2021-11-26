using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

public class LSLTestingOutput : MonoBehaviour
{
    [SerializeField] private string StreamName = "Headset Position";
    private string StreamType = "";
    
    [Space(10)]
    [SerializeField] private Transform _trackedObject;
    

    private liblsl.StreamOutlet outlet;
    
    
    
    void Awake()
    {
        liblsl.StreamInfo streamInfo = new liblsl.StreamInfo(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, liblsl.channel_format_t.cf_float32);
        liblsl.XMLElement channels = streamInfo.desc().append_child("channels");
        
        channels.append_child("channel").append_child_value("label", "X");
        channels.append_child("channel").append_child_value("label", "Y");
        channels.append_child("channel").append_child_value("label", "Z");
        
        outlet = new liblsl.StreamOutlet(streamInfo);
    }

    void FixedUpdate()
    {
        Vector3 trackedObjectPosition = _trackedObject.position;
        float[] positionData = {trackedObjectPosition.x, trackedObjectPosition.y, trackedObjectPosition.z};
        
        outlet.push_sample(positionData);
    }
}
