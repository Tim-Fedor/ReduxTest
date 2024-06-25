using System;
using UnityEngine;

namespace Modules.PlayerTapController
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private float _defaultRotateSpeed = 5f;
        [SerializeField] private float _distanceThreshold = 0.1f;
        
        private Rigidbody _rb;
        private Animator _animator;
        private int _defaultLives;
        private float _defaultSpeed;

        public float Speed => _movementController.CurrentSpeed;
        public int Lives => _healthController.CurrentLives;

        public event Action PlayerDied;
        public event Action PlayerFinishPath;

        private MovementController _movementController;
        private HealthController _healthController;
        private JumpController _jumpController;

        public void SetupDefaultValues(int defaultLives, float defaultSpeed)
        {
            _defaultSpeed = defaultSpeed;
            _defaultLives = defaultLives;
            
            _movementController = new MovementController(_rb, _defaultSpeed, _defaultRotateSpeed, _distanceThreshold);
            _healthController = new HealthController(_defaultLives, _animator);
            _jumpController = new JumpController(_rb, _animator);

            _movementController.OnFinalPoint += GetToTheFinalPoint;
            _healthController.ZeroHealth += OnZeroHealth;
        }
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }
        
        //Movement Region start
        public void InitializePath(Waypoints waypointController)
        {
            _movementController.Initialize(waypointController);
        }
        
        private void Update()
        {
            _movementController.UpdateMovement();
            
            if (Input.GetMouseButtonDown(0))
            {
                _jumpController.HandleJumpInput();
            }
        }

        private void FixedUpdate()
        {
            _movementController.FixedUpdateMovement();
        }
        
        public void GoToNextWaypoint()
        {
            _movementController.GoToNextWaypoint();
            transform.position = _movementController.CurrentWaypoint.position;
        }

        public Transform GetCurrentWaypoint()
        {
            return _movementController.CurrentWaypoint;
        }

        private void GetToTheFinalPoint()
        {
            PlayerFinishPath?.Invoke();
        }

        public void ApplySpeedChange(float newSpeed, float durationS)
        {
            _movementController.ApplySpeedChange(newSpeed, durationS);
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            _jumpController.OnCollisionEnter(collision);
        }
        //Movement Region end

        //Health Region start
        private void OnZeroHealth()
        {
            PlayerDied?.Invoke();
        }
        
        public void ApplyHealthChange(int newHealth, bool isForce = false)
        {
            _healthController.ApplyHealthChange(newHealth, isForce);
        }

        public void ApplyIDDQDMode(float durationMS)
        {
            _healthController.ApplyIDDQDMode(durationMS);
        }
        //Health Region end
        
        private void OnDestroy()
        {
            _movementController.OnFinalPoint -= GetToTheFinalPoint;
            _healthController.ZeroHealth -= OnZeroHealth;
        }
    }
}
