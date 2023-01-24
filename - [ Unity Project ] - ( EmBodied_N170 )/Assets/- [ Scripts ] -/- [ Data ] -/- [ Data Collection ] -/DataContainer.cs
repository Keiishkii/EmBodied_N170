using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// The DataContainer class is the container class for all recorded data within the program,
        /// This class is serialised and written to a JSON file at the end of the experiment.
        /// </summary>
        public class DataContainer
        {
            // The time of the experiments start.
            public string dateTime;

            // A list of block data, each describing the content of a single block.
            public readonly List<BlockData> blockData = new List<BlockData>();
            // A list of time stamped events of things happening in the program.
            public readonly List<DataCollectionEvent_Interface> dataEvents = new List<DataCollectionEvent_Interface>();

            // A list of input data, recorded for each button press the user makes
            public readonly List<InputSampleData> inputSamples = new List<InputSampleData>(); // Controller Inputs
            // A list of frame data for transforms of the head, camera, and hands. 
            public readonly List<TransformSampleData> transformSamples = new List<TransformSampleData>(); // Transform Data
        }
    }
}