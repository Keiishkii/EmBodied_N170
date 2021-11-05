using System.Collections;
using System.Collections.Generic;
using LSL;
using UnityEngine;

public class LSLTestingInput : MonoBehaviour
{
    [SerializeField] private string _streamName = "";
    //private string _streamType = "";
    
    [Space(10)]
    [SerializeField] private Transform _displayObject;
    [SerializeField] private Vector3 _offset;
    
    private int _channelCount = 0;
    
    StreamInfo[] _streamInfos;
    StreamInlet _streamInlet;
    
    
    
    
    
    void FixedUpdate()
    {
        if (_streamInlet == null)
        {
            /* - This creates a connection to a potential stream given a property, my guess is that the property is a dictionary look up as it can take a key "name" or "type" which allows you to search for the stream given either of them. */
            
            _streamInfos = LSL.LSL.resolve_stream("name", _streamName, 1, 0);
            /*_streamInfos = LSL.LSL.resolve_stream("type", _streamType, 1, 0);*/
            
            if (_streamInfos.Length > 0)
            {
                _streamInlet = new StreamInlet(_streamInfos[0]);
                _channelCount = _streamInlet.info().channel_count();
                _streamInlet.open_stream();
            }
        }

        if (_streamInlet != null)
        {
            float[] sample = new float[_channelCount];
            double lastTimeStamp = _streamInlet.pull_sample(sample, 0.0f);
            if (lastTimeStamp != 0.0)
            {
                Process(sample, lastTimeStamp);
                while ((lastTimeStamp = _streamInlet.pull_sample(sample, 0.0f)) != 0)
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
        Vector3 position = new Vector3((newSample[0]), (newSample[1]), (newSample[2]));
        _displayObject.position = position + _offset;
    }
}