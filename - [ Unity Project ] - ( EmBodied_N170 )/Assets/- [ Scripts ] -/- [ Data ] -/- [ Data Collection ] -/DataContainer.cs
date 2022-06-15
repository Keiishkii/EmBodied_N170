using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataCollection
{
    [Serializable]
    public class DataContainer
    {
        public readonly List<BlockData> blockData = new List<BlockData>();
        public readonly List<DataCollectionEvent_Interface> dataEvents = new List<DataCollectionEvent_Interface>();
    }
}