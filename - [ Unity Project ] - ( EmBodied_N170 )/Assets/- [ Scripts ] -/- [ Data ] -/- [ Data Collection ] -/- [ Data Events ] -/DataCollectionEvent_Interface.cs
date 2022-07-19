using System;
using UnityEngine;

namespace Data {
    namespace DataCollection
    {
        [Serializable]
        public abstract class DataCollectionEvent_Interface
        {
            public float timeSinceProgramStart;

            protected DataCollectionEvent_Interface()
            {
                timeSinceProgramStart = Time.timeSinceLevelLoad;
            }
        }
    }
}