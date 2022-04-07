/*
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameController_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Game Controller");
        GameController targetScript = (GameController) target;

        if (!Application.isPlaying)
        {
            base.OnInspectorGUI();
        }
        else
        {
            if (GUILayout.Button("Increment State"))
            {
                targetScript.gameState++;
            }
        }
    }
}
#endif
*/