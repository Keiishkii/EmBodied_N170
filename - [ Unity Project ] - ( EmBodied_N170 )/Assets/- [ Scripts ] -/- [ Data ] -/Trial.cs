using System;
using UnityEngine;

namespace Data 
{
    namespace Input
    {
        /// <summary>
        /// A class used to describe the setup and behaviour of a given trial.
        /// Specifically who the player will interact with, and what they will be holding as there interaction medium.
        /// </summary>
        [Serializable]
        public class Trial
        {
            // The prefab of the non player character sitting in the front room.
            public GameObject roomA_NPCAvatar;
            // The prefab of the non player character sitting in the back room.
            public GameObject roomB_NPCAvatar;
            // The prefab of the held object the player will be holding in the hand.
            public GameObject heldObject;
        }
    }
}