using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _headsetTransform;
    [SerializeField] private Transform _leftHandTransform, _rightHandTransform;

    [Space] 
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 eulerRotationOffset;
    
    private GameObject _heldItem;
    
    
    
    
    
    public void CreateHeldItem(GameObject heldObject)
    {
        _heldItem = Instantiate(heldObject, _rightHandTransform);

        _heldItem.transform.localPosition = positionOffset;
        _heldItem.transform.rotation = Quaternion.Euler(eulerRotationOffset);
    }
    
    public void DestroyHeldItem()
    {
        Destroy(_heldItem);
    }
    
    
    
    
    
    
    /*
    
    
    
    
    
    public IEnumerator TestDistanceForPlayerApproach(NPCController npcController, float distance)
    {
        Transform npcHeadTransform = npcController.GetHeadTransform();
        Vector3 position, npcFacePosition;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            npcFacePosition = npcHeadTransform.position;
            npcFacePosition = new Vector3(npcFacePosition.x, 0, npcFacePosition.z);

            yield return null;
        } while (Vector3.SqrMagnitude(position - npcFacePosition) > Mathf.Pow(distance, 2));

        npcController.LookAt(true);

        GameController.Instance.ChosenRoom = (position.x > 0) ? Room_Enum.ROOM_A : Room_Enum.ROOM_B;
        Debug.Log("Player has entered room " + ((position.x > 0) ? Room_Enum.ROOM_A : Room_Enum.ROOM_B).ToString());
        GameController.Instance.gameState = GameState_Enum.PLAYER_APPROACHED;
    }

    public IEnumerator TestDistanceForWalkingAway(NPCController npcController, float distance)
    {
        Transform npcHeadTransform = npcController.GetHeadTransform();
        Vector3 position, npcFacePosition;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            npcFacePosition = npcHeadTransform.position;
            npcFacePosition = new Vector3(npcFacePosition.x, 0, npcFacePosition.z);

            yield return null;

            float distanceA = Vector3.SqrMagnitude(position - npcFacePosition);
            float distanceB = Mathf.Pow(distance, 2);
        } while (Vector3.SqrMagnitude(position - npcFacePosition) < Mathf.Pow(distance, 2));
        
        npcController.LookAt(false);
        
        GameController.Instance.gameState = GameState_Enum.PLAYER_DECISION;
    }

    public IEnumerator TestDistanceToCenter(float distance)
    {
        Vector3 position;
        
        do
        {
            position = _headsetTransform.position;
            position = new Vector3(position.x, 0, position.z);
            
            yield return null;
        } while (Vector3.SqrMagnitude(position) > Mathf.Pow(distance, 2));
        
        GameController.Instance.gameState = GameState_Enum.PLAYER_RETURNED;
    }
    */

    /*
    public Transform GetHeadsetTransform()
    {
        return _headsetTransform;
    }
    */
}