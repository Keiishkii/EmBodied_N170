using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLInput : MonoBehaviour
{
    /*
    public string StreamType = "EEG";
    public float scaleInput = 0.1f;
*/
    private string StreamName = "A";
    private string StreamType = "B";
    private int channelCount = 0;
    
    StreamInfo[] streamInfos;
    StreamInlet streamInlet;
    float[] sample;
    
    
    
    
    
    void Update()
    {
        if (streamInlet == null)
        {
            streamInfos = LSL.LSL.resolve_stream("type", StreamType, 1, 0);
            if (streamInfos.Length > 0)
            {
                streamInlet = new StreamInlet(streamInfos[0]);
                channelCount = streamInlet.info().channel_count();
                streamInlet.open_stream();
            }
        }

        if (streamInlet != null)
        {
            sample = new float[channelCount];
            double lastTimeStamp = streamInlet.pull_sample(sample, 0.0f);
            if (lastTimeStamp != 0.0)
            {
                Process(sample, lastTimeStamp);
                while ((lastTimeStamp = streamInlet.pull_sample(sample, 0.0f)) != 0)
                {
                    Process(sample, lastTimeStamp);
                }
            }
        }
        else
        {
            Debug.Log("StreamInput is still null");
        }
    }
        
    void Process(float[] newSample, double timeStamp)
    {
        Vector3 position = new Vector3((newSample[0] - 0.5f), (newSample[1] - 0.5f), (newSample[2] -0.5f)) + new Vector3(0, 2, 0);
        gameObject.transform.position = position;
    }
}