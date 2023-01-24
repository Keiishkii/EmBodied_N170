using System;
using System.Collections.Generic;

namespace Data 
{
    namespace DataCollection
    {
        /// <summary>
        /// This class used used to record the data from a given block.
        /// Mainly stores the contents of trials.
        /// </summary>
        public class BlockData
        {
            // The correct room the player should be interacting with for this block.
            public Enums.Room activeRoom;
            // A list of classes storing data for individual trials within this block.
            public readonly List<TrialData> trialData = new List<TrialData>();
        }
    }
}