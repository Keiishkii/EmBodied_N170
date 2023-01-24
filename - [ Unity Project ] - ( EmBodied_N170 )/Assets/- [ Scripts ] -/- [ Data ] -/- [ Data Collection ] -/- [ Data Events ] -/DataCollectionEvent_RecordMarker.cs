using System;
using Unity.Mathematics;
using UnityEngine;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// A child of the DataCollectionEvent_Interface class, this is used to write messages as events.
        /// </summary>
        [Serializable]
        public class DataCollectionEvent_RecordMarker : DataCollectionEvent_Interface
        {
            // A string for output messages as events, used from notifying changes like lights on or when the user completes a trial.
            public string record;
        }
    }
}