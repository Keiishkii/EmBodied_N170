using System;
using Unity.Mathematics;
using UnityEngine;

namespace DataCollection
{
    [Serializable]
    public class DataCollectionEvent_RecordMarker : DataCollectionEvent_Interface
    {
        public float timeSinceProgramStart;
        public string currentState;
        
        public DataCollectionEvent_RecordMarker()
        {
            type = nameof(DataCollectionEvent_RecordMarker);
        }
    }
}