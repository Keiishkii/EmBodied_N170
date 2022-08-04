using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data {
    namespace DataCollection
    {
        public class DataContainer
        {
            public string dateTime;

            public readonly List<BlockData> blockData = new List<BlockData>();
            public readonly List<DataCollectionEvent_Interface> dataEvents = new List<DataCollectionEvent_Interface>();

            public readonly List<InputSampleData> inputSamples = new List<InputSampleData>(); // Controller Inputs
            public readonly List<TransformSampleData> transformSamples = new List<TransformSampleData>(); // Transform Data
        }
    }
}