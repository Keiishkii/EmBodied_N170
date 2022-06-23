using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Session Setup/Session Format Object")]
    public class SessionFormat : ScriptableObject
    {
        [SerializeReference] public List<Block> blocks = new List<Block>();
        
        public float approachDistance;
        public Enums.Handedness participantHandedness;
    }
}
