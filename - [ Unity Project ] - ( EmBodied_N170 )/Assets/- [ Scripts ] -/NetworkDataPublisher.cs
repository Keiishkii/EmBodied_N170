using _LSL;
using UnityEngine;

public class NetworkDataPublisher : MonoBehaviour
{
#if PLATFORM_STANDALONE_WIN

    [SerializeField] private LSLOutput_MarkerStream _markerStream;
    public void PublishMarkerToNetwork(in string marker)
    {
        _markerStream.PublishMarkerToNetwork(marker);
    }
#else
    private void Awake()
    {
        gameObject.SetActive(false);
    }
#endif
}
