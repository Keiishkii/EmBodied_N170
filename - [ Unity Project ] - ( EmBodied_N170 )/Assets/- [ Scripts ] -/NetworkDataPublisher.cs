using System.Collections;
using System.Collections.Generic;
using _LSL;
using UnityEngine;

public class NetworkDataPublisher : MonoBehaviour
{
    [SerializeField] private LSLOutput_MarkerStream _markerStream;

    public void PublishMarkerToNetwork(in string marker)
    {
        _markerStream.PublishMarkerToNetwork(marker);
    }
}
