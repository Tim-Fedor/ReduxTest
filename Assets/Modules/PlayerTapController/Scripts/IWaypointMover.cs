using UnityEngine;

namespace Modules.PlayerTapController
{
    public interface IWaypointMover
    {
        void InitializePath(Waypoints waypointController);
        void GoToNextWaypoint();
        Transform GetCurrentWaypoint();
    }
}