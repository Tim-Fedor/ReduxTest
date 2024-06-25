using UnityEngine;

namespace Modules.BoostGeneration
{
    [CreateAssetMenu(fileName = "BoostData", menuName = "ScriptableObjects/BoostData", order = 2)]
    public class BoostData : ScriptableObject
    {
        public int ID;
        public string BoostModelName;
        [Range(0, 100)]
        public float ChanceToSpawn;
        public float FloatValue;
        public int IntValue;
    }
}
