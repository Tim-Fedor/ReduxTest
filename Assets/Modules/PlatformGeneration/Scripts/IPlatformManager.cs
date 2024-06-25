using UnityEngine;

namespace Modules.PlatformGeneration
{
    public interface IPlatformManager
    {
        public Transform GetNextWaypoint(Transform waypoint);
        public PlatformData[] GetAllDangerPlatformPassedBy(Transform waypoint);
    }
}