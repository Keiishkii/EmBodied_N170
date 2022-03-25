using UnityEngine;

namespace _Debug
{
    [System.Serializable]
    public class GizmoRenderer_GizmoType
    {
        public string label;
        public Color colour;
    }

    [System.Serializable]
    public class GizmoRenderer_GizmoMesh : GizmoRenderer_GizmoType
    {
        public Mesh mesh;

        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.Euler(0, 0, 0);
        public Vector3 scale = Vector3.one;
    }

    [System.Serializable]
    public class GizmoRenderer_GizmoCube : GizmoRenderer_GizmoType
    {
        public Vector3 position = Vector3.zero;
        public Vector3 scale = Vector3.one;
    }

    [System.Serializable]
    public class GizmoRenderer_GizmoSphere : GizmoRenderer_GizmoType
    {
        public Vector3 position = Vector3.zero;
        public float radius = 0.5f;
    }
}