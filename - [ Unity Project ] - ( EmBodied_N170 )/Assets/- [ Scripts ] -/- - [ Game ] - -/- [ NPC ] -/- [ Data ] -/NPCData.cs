using NPC_Controller;
using UnityEngine;

public class NPCData
{    
    public GameObject gameObject;
    public NPCController_StateMachine npcController;
    public NPCBoneReferences npcBoneReferences;
    public NPCIKReferences npcIKReferences;

    public void Populate(GameObject npcGameObject)
    {
        gameObject = npcGameObject;
        npcController = gameObject.GetComponent<NPCController_StateMachine>();
        npcBoneReferences = gameObject.GetComponent<NPCBoneReferences>();
        npcIKReferences = gameObject.GetComponent<NPCIKReferences>();
    }
}