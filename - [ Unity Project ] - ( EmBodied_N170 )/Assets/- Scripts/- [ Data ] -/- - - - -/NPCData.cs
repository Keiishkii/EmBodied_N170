using UnityEngine;

public class NPCData
{    
    public GameObject gameObject;
    public NPCController npcController;
    public NPCBoneReferences npcBoneReferences;
    public NPCIKReferences npcIKReferences;

    public void Populate(GameObject npcGameObject)
    {
        gameObject = npcGameObject;
        npcController = gameObject.GetComponent<NPCController>();
        npcBoneReferences = gameObject.GetComponent<NPCBoneReferences>();
        npcIKReferences = gameObject.GetComponent<NPCIKReferences>();
    }
}