using System;
using System.Collections.Generic;

namespace DataCollection
{
    [Serializable]
    public class BlockData
    {
        public readonly List<TrialData> trialData = new List<TrialData>();
    }
}