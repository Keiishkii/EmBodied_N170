using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Controller
{
    public abstract class NPCState_Interface
    {
        public virtual void OnEnterState(in NPCController_StateMachine stateMachine) { }
        public virtual void Update(in NPCController_StateMachine stateMachine) { }
        public virtual void OnExitState(in NPCController_StateMachine stateMachine) { }
        
        public virtual void OnDrawGizmos(in NPCController_StateMachine stateMachine) { }
    }
}
