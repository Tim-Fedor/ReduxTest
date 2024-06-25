using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.PlayerTapController
{
    public class MovementController
    {
        private Rigidbody _rb;
        private float _defaultSpeed;
        private float _defaultRotateSpeed;
        private float _distanceThreshold;
        private Waypoints _waypoints;
        private Transform _currentWaypoint;
        
        private float _currentSpeed;

        public event Action OnFinalPoint;

        public float CurrentSpeed => _currentSpeed;
        public Transform CurrentWaypoint => _currentWaypoint;

        public MovementController(Rigidbody rb, float defaultSpeed, float defaultRotateSpeed, float distanceThreshold)
        {
            _rb = rb;
            _defaultSpeed = defaultSpeed;
            _defaultRotateSpeed = defaultRotateSpeed;
            _distanceThreshold = distanceThreshold;

            _currentSpeed = _defaultSpeed;
        }

        public void Initialize(Waypoints waypoints)
        {
            _waypoints = waypoints;
            GoToNextWaypoint();
        }

        public void UpdateMovement()
        {
            if (_waypoints == null) return;
            if(_currentWaypoint == null) GoToNextWaypoint();
            
            if (Vector3.Distance(_rb.transform.position, new Vector3(_currentWaypoint.position.x, _rb.transform.position.y, _currentWaypoint.position.z)) < _distanceThreshold)
            {
                _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
                if (_waypoints.IsWaypointFinal(_currentWaypoint))
                {
                    OnFinalPoint?.Invoke();
                }
            }
            RotateTowardsWaypoint();
        }

        public void GoToNextWaypoint()
        {
            _currentWaypoint = _waypoints.GetNextWaypoint(_currentWaypoint);
        }

        public void FixedUpdateMovement()
        {
            if (_waypoints == null) return;
            if(_currentWaypoint == null) GoToNextWaypoint();

            Vector3 targetPosition = new Vector3(_currentWaypoint.position.x, _rb.transform.position.y, _currentWaypoint.position.z);
            Vector3 newPosition = Vector3.MoveTowards(_rb.transform.position, targetPosition, _currentSpeed * Time.fixedDeltaTime);

            _rb.MovePosition(newPosition);
        }

        private void RotateTowardsWaypoint()
        {
            Vector3 directionToWaypoint = _currentWaypoint.position - _rb.transform.position;
            directionToWaypoint.y = 0;
            directionToWaypoint.Normalize();

            Quaternion rotationGoal = Quaternion.LookRotation(directionToWaypoint);
            _rb.transform.rotation = Quaternion.Slerp(_rb.transform.rotation, rotationGoal, _defaultRotateSpeed * Time.deltaTime);
        }
        
        public async void ApplySpeedChange(float newSpeed, float durationS)
        {
            if (_currentSpeed != _defaultSpeed) return;

            _currentSpeed = newSpeed;
            await UniTask.WaitForSeconds(durationS);
            _currentSpeed = _defaultSpeed;
        }
    }
}