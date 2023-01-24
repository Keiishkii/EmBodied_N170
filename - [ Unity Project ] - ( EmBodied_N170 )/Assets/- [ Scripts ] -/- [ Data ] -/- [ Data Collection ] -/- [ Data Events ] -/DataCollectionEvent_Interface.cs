using System;
using UnityEngine;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// This class is the interface class for all DataCollectionEvents.
        /// On its own it doesn't do much.
        /// </summary>
        [Serializable]
        public abstract class DataCollectionEvent_Interface
        {
            // Time of event invocation
            public float timeSinceProgramStart;

            protected DataCollectionEvent_Interface()
            {
                timeSinceProgramStart = Time.timeSinceLevelLoad;
            }
        }
    }
}