#if UNITY_EDITOR
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    namespace _Debug
    {
        [CustomEditor(typeof(DebugGizmos))]
        public class DebugGizmosEditor : Editor
        {
            private GizmoType_Enum _selectedGizmoType;
            
            
            
            public override void OnInspectorGUI()
            {
                DebugGizmos targetScript = (DebugGizmos) target;
                
                base.OnInspectorGUI();
                
                GUILayout.BeginHorizontal();
                
                _selectedGizmoType = (GizmoType_Enum) EditorGUILayout.EnumPopup("Gizmo Type:", _selectedGizmoType);
                if (GUILayout.Button("Add Gizmo"))
                {
                    switch (_selectedGizmoType)
                    {
                        case GizmoType_Enum.MESH:
                        {
                            targetScript.gizmoList.Add(new GizmoRenderer_GizmoMesh());
                        } break;
                        case GizmoType_Enum.CUBE:
                        {
                            targetScript.gizmoList.Add(new GizmoRenderer_GizmoCube());
                        } break;
                        case GizmoType_Enum.SPHERE:
                        {
                            targetScript.gizmoList.Add(new GizmoRenderer_GizmoSphere());
                        } break;
                    }
                }
                
                GUILayout.EndHorizontal();
            }
        }
    }
#endif