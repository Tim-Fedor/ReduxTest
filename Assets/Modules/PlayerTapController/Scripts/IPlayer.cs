using System;

namespace Modules.PlayerTapController
{
    public interface IPlayer : IWaypointMover
    {
        void SetupDefaultValues(int defaultLives, float defaultSpeed);
        void ApplySpeedChange(float newSpeed, float duration);
        void ApplyHealthChange(int newHealth, bool isForce = false);
        void ApplyIDDQDMode(float durationMS);
        int Lives { get; }
        float Speed { get; }
        event Action PlayerDied;
        event Action PlayerFinishPath;
    }
}