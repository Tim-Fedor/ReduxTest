using UnityEngine;

namespace Modules.PlatformGeneration
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings", order = -1)]
    public class GameSettings : ScriptableObject
    {
        public string PlayerModelName;
        public int PlayerDefaultLives;
        public float PlayerDefaultSpeed;
        public string WinScreenModelName;
        public string LoseScreenModelName;
    }
}
