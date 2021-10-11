using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLOutput : MonoBehaviour
{
    private StreamOutlet outlet;
    private float[] currentSample;

    private string StreamName = "A";
    private string StreamType = "B";

    
    
    
    
    void Start()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, channel_format_t.cf_float32);
        XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "X");
        chans.append_child("channel").append_child_value("label", "Y");
        chans.append_child("channel").append_child_value("label", "Z");
        outlet = new StreamOutlet(streamInfo);
        currentSample = new float[3];
    }

    void FixedUpdate()
    {
        Vector3 pos = gameObject.transform.position;
        currentSample[0] = pos.x;
        currentSample[1] = pos.y;
        currentSample[2] = pos.z;
        outlet.push_sample(currentSample);

        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += new Vector3(0, 1f * Time.fixedDeltaTime, 0);
        }
    }
}