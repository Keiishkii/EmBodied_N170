using System;
using Unity.Mathematics;
using UnityEngine;

namespace Data {
    namespace DataCollection
    {
        [Serializable]
        public class DataCollectionEvent_RecordMarker : DataCollectionEvent_Interface
        {
            public string record;
        }
    }
}