using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Debug
{
    public class DebugGizmos : MonoBehaviour
    {
        [SerializeField] private bool _renderGizmos = true;
        [SerializeReference] public List<GizmoRenderer_GizmoType> gizmoList = new List<GizmoRenderer_GizmoType>();


        private void OnDrawGizmos()
        {
            if (_renderGizmos)
            {
                foreach (GizmoRenderer_GizmoType gizmoType in gizmoList)
                {
                    switch (gizmoType)
                    {
                        case GizmoRenderer_GizmoMesh meshGizmo:
                        {
                            if (meshGizmo.mesh != null)
                            {
                                Gizmos.color = meshGizmo.colour;
                                Gizmos.DrawMesh(meshGizmo.mesh, meshGizmo.position, meshGizmo.rotation, meshGizmo.scale);
                            }
                        }
                            break;
                        case GizmoRenderer_GizmoCube cubeGizmo:
                        {
                            Gizmos.color = cubeGizmo.colour;
                            Gizmos.DrawWireCube(cubeGizmo.position, cubeGizmo.scale);
                        }
                            break;
                        case GizmoRenderer_GizmoSphere sphereGizmo:
                        {
                            Gizmos.color = sphereGizmo.colour;
                            Gizmos.DrawWireSphere(sphereGizmo.position, sphereGizmo.radius);
                        }
                            break;
                    }
                }
            }
        }
    }
}