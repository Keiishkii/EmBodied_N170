using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data 
{
    namespace Input
    {
        /// <summary>
        /// The input class object for designing and then replaying session via scriptable objects.
        /// The purpose of the structure is to allow for comparable tests, where individuals can perform the same test with identical conditions, or for understanding what data may mean from looking back at the inputs.
        /// </summary>
        [CreateAssetMenu(menuName = "Session Setup/Session Format Object")]
        public class SessionFormat : ScriptableObject
        {
            // A list of objects describing the setup and completions of a trials blocks.
            [SerializeReference] public List<Block> blocks = new List<Block>();

            // The distance the player needs to approach an NPC for it to look up and interact with the player.
            public float approachDistance;
            // The handedness the players avatar will be, this sets what hand the player is expected to interact within the trials with.
            public Enums.Handedness participantHandedness;
        }
    }
}
