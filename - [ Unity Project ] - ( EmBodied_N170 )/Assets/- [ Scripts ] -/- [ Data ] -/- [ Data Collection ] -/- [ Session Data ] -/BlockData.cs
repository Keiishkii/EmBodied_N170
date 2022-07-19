using System;
using System.Collections.Generic;

namespace Data {
    namespace DataCollection
    {
        public class BlockData
        {
            public Enums.Room activeRoom;
            public readonly List<TrialData> trialData = new List<TrialData>();
        }
    }
}