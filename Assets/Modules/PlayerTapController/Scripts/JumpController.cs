using DG.Tweening;
using UnityEngine;

namespace Modules.PlayerTapController
{
    public class JumpController
    {
        private Rigidbody _rb;
        private Animator _animator;
        private bool _isJumping = false;
        private bool _isDoubleJumping = false;

        public JumpController(Rigidbody rb, Animator animator)
        {
            _rb = rb;
            _animator = animator;
        }

        public void HandleJumpInput()
        {
            if (_isJumping && !_isDoubleJumping)
            {
                DoubleJump();
            }
            else if (!_isJumping)
            {
                Jump();
            }
        }

        private void Jump()
        {
            _rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            _isJumping = true;
            SetAnimationJump(1, 1.5f);
        }

        private void DoubleJump()
        {
            _rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            _isDoubleJumping = true;
        }

        private void SetAnimationJump(float value, float duration)
        {
            DOTween.To(() => _animator.GetFloat("IsJumping"), x => _animator.SetFloat("IsJumping", x), value, duration);
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                _isJumping = false;
                _isDoubleJumping = false;
                SetAnimationJump(0, 1f);
            }
        }
    }
}