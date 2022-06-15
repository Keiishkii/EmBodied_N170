using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoRenderer_DarkRoom : MonoBehaviour
{
    [SerializeField] private Bounds _darkRoomBounds;
    
    private void OnDrawGizmos()
    {
        Vector3 
            corner_1 = new Vector3(_darkRoomBounds.min.x, _darkRoomBounds.min.y, _darkRoomBounds.min.z),
            corner_2 = new Vector3(_darkRoomBounds.max.x, _darkRoomBounds.min.y, _darkRoomBounds.min.z), 
            corner_3 = new Vector3(_darkRoomBounds.max.x, _darkRoomBounds.min.y, _darkRoomBounds.max.z), 
            corner_4 = new Vector3(_darkRoomBounds.min.x, _darkRoomBounds.min.y, _darkRoomBounds.max.z);

        Vector3 center = new Vector3(_darkRoomBounds.center.x, _darkRoomBounds.min.y, _darkRoomBounds.center.z);
        
        Gizmos.DrawLine(corner_1, corner_2);
        Gizmos.DrawLine(corner_2, corner_3);
        Gizmos.DrawLine(corner_3, corner_4);
        Gizmos.DrawLine(corner_4, corner_1);
        
        Gizmos.DrawLine(center + new Vector3(0.5f, 0, 0.5f), center + new Vector3(-0.5f, 0, -0.5f));
        Gizmos.DrawLine(center + new Vector3(-0.5f, 0, 0.5f), center + new Vector3(0.5f, 0, -0.5f));
    }
}
