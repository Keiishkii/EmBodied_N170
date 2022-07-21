#if PLATFORM_STANDALONE_WIN
using LSL;
#endif

using UnityEngine;

namespace _LSL
{
    // Abstract class used for writing LSL streams. Stores the values it will be writing, and the name of the stream and its description.
    public abstract class LSLOutput<T> : MonoBehaviour
    {
#if PLATFORM_STANDALONE_WIN
        [SerializeField] protected string _streamName = "Stream Name";
        [SerializeField] protected string _streamType = "Stream Type";

        protected StreamOutlet _outlet;
        protected T[] _currentSample;
        
        protected static void PushOutput(in StreamOutlet outlet, in char[] sample) { outlet.push_sample(sample); }
        protected static void PushOutput(in StreamOutlet outlet, in double[] sample) { outlet.push_sample(sample); }
        protected static void PushOutput(in StreamOutlet outlet, in float[] sample) { outlet.push_sample(sample); }
        protected static void PushOutput(in StreamOutlet outlet, in int[] sample) { outlet.push_sample(sample); }
        protected static void PushOutput(in StreamOutlet outlet, in short[] sample) { outlet.push_sample(sample); }
        protected static void PushOutput(in StreamOutlet outlet, in string[] sample) { outlet.push_sample(sample); }
#endif
    }
}