using UnityEngine;

namespace Modules.PlatformGeneration
{
    [CreateAssetMenu(fileName = "PlatformData", menuName = "ScriptableObjects/PlatformData", order = 1)]
    public class PlatformData : ScriptableObject
    {
        public int ID;
        public string ViewName;
        public string PlatformModelName;
        public bool IsDangerBlock;
        [Range(0f, 100f)]
        public float ChanceToSpawn;
        public float Length;
        public Vector3 Rotation;
    }
}
