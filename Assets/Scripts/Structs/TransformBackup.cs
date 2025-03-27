using Unity.VisualScripting;
using UnityEngine;

namespace SkyDragonHunter.Structs
{
    [System.Serializable]
    public struct TransformBackup
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public void ApplyTo(Transform t)
        {
            t.position = position;
            t.rotation = rotation;
            t.localScale = scale;
        }

        public static TransformBackup FromTransform(Transform t)
        {
            return new TransformBackup
            {
                position = t.position,
                rotation = t.rotation,
                scale = t.localScale
            };
        }

    } // public struct TransformBackup
} // namespace SkyDragonHunter.Utility