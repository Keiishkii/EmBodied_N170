/*
#if UNITY_EDITOR
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.Experimental.TerrainAPI;
    using UnityEngine;

    [CustomEditor(typeof(NPCController))]
    public class NPCControllerEditor : Editor
    {
        private bool _isLookingUp;
        
        public override void OnInspectorGUI()
        {
            NPCController targetScript = (NPCController) target;
            
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
            }
            else
            {
                if (GUILayout.Button("Toggle Look At"))
                {
                    targetScript.LookAt(_isLookingUp);
                    _isLookingUp = !_isLookingUp;
                }
            }
        }
    }
#endif
*/