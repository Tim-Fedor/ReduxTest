using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Modules.PlayerTapController
{
    public class HealthController
    {
        private int _lives;
        private Animator _animator;
        private bool _isIDDQD = false;

        public event Action ZeroHealth;
        public int CurrentLives => _lives;

        public HealthController(int defaultLives, Animator animator)
        {
            _lives = defaultLives;
            _animator = animator;
        }
        
        public async void ApplyIDDQDMode(float durationMS)
        {
            if (_isIDDQD) return;

            _animator.Play("IDDQDState");
            _isIDDQD = true;
            await UniTask.WaitForSeconds(durationMS);
            _isIDDQD = false;
            _animator.Play("NoEffects");
        }

        public void ApplyHealthChange(int newHealth, bool isForce)
        {
            if (_isIDDQD && newHealth < _lives && !isForce) return;

            if (newHealth < _lives)
            {
                _animator.Play("GetDamage");
            }

            _lives = newHealth;

            if (_lives <= 0)
            {
                ZeroHealth?.Invoke();
            }
        }
    }
}