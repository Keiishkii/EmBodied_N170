#if UNITY_EDITOR
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameController targetScript = (GameController) target;
            
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
            }
            else
            {
                
            }
        }
    }
#endif