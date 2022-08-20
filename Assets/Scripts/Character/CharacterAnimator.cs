using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	[RequireComponent(typeof(Animator))]
	public class CharacterAnimator : MonoBehaviour, IPausable
	{
		[SerializeField] private float m_direction = 1f;
		[SerializeField] private CharacterController2D m_controller;

		private static readonly int animVerticalHash = Animator.StringToHash("Vertical");
		private static readonly int animRunningHash = Animator.StringToHash("Running");
		private static readonly int animDeadHash = Animator.StringToHash("Dead");
		private static readonly int animAttackHash = Animator.StringToHash("Attack");
		private static readonly int animDamageHash = Animator.StringToHash("Damage");
		private static readonly int animRollHash = Animator.StringToHash("Roll");
		private static readonly int animLandingHash = Animator.StringToHash("Landing");

		private Animator currentAnimator;

		private void Update()
		{
			currentAnimator.SetBool(animRunningHash, m_controller.IsGrounded && Mathf.Abs(m_controller.HorizontalMovement) > 0.1);
			currentAnimator.SetFloat(animVerticalHash, m_controller.VerticalVelocity);
			UpdateDirection();
		}

		private void OnEnable()
		{
			currentAnimator = GetComponent<Animator>();
			m_controller.OnAttackEvent.AddListener(OnAttack);
			m_controller.OnRollingEvent.AddListener(OnRolling);
			m_controller.OnDamageEvent.AddListener(OnDamage);
			m_controller.OnDeathEvent.AddListener(OnDeath);
			//m_controller.OnJumpEvent.AddListener(OnJump);
			m_controller.OnLandingEvent.AddListener(OnLanding);
		}

		private void OnDisable()
		{
			m_controller.OnAttackEvent.RemoveListener(OnAttack);
			m_controller.OnRollingEvent.RemoveListener(OnRolling);
			m_controller.OnDamageEvent.RemoveListener(OnDamage);
			m_controller.OnDeathEvent.RemoveListener(OnDeath);
			//m_controller.OnJumpEvent.RemoveListener(OnJump);
			m_controller.OnLandingEvent.RemoveListener(OnLanding);
		}

		private void OnAttack()
		{
			currentAnimator.ResetTrigger(animDamageHash);
			currentAnimator.ResetTrigger(animRollHash);
			currentAnimator.ResetTrigger(animLandingHash);
			currentAnimator.SetTrigger(animAttackHash);
		}

		private void OnRolling()
		{
			currentAnimator.ResetTrigger(animAttackHash);
			currentAnimator.ResetTrigger(animDamageHash);
			currentAnimator.ResetTrigger(animLandingHash);
			currentAnimator.SetTrigger(animRollHash);
		}
		private void OnDamage()
		{
			currentAnimator.ResetTrigger(animAttackHash);
			currentAnimator.ResetTrigger(animRollHash);
			currentAnimator.ResetTrigger(animLandingHash);
			currentAnimator.SetTrigger(animDamageHash);
		}
		private void OnLanding()
		{
			currentAnimator.ResetTrigger(animDamageHash);
			currentAnimator.ResetTrigger(animRollHash);
			currentAnimator.ResetTrigger(animAttackHash);
			currentAnimator.SetTrigger(animLandingHash);
		}

		private void OnDeath()
		{
			currentAnimator.ResetTrigger(animAttackHash);
			currentAnimator.ResetTrigger(animDamageHash);
			currentAnimator.ResetTrigger(animRollHash);
			currentAnimator.ResetTrigger(animLandingHash);
			currentAnimator.SetBool(animDeadHash, true);
		}

		private void UpdateDirection()
		{
			Vector3 theScale = transform.localScale;
			if (m_controller.PrimaryDirection > 0 && theScale.x != m_direction)
			{
				theScale.x = m_direction;
				transform.localScale = theScale;
			}
			else if (m_controller.PrimaryDirection < 0 && theScale.x != -m_direction)
			{
				theScale.x = -m_direction;
				transform.localScale = theScale;
			}
		}

		public void OnPause(bool pause)
		{
			enabled = !pause;
			currentAnimator.enabled = !pause;
		}
	}
}