using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class CharacterController2D : CharacterBase
	{
		[Space]
		[Header("Controller")]
		[SerializeField] private bool m_AirControl = false;
		[SerializeField] private bool m_OneJumpAllowed = false;

		[Header("Movement Setup")]
		[SerializeField] private float m_JumpForce = 5f;
		[SerializeField] private float m_RunSpeed = 5f;
		[SerializeField] private float m_RollingSpeed = 3f;
		[SerializeField] private float m_RollingDuration = 0.5f;
		[SerializeField] private float m_RollingCooldown = 0.6f;
		[SerializeField] private float m_FallingTreshold = 0.1f;
		[SerializeField] private float m_RunTreshold = 0.1f;
		[Range(0, .5f)] [SerializeField] private float m_MovementSmoothing = .05f;
		[Range(0, .5f)] [SerializeField] private float m_AirSmoothing = .1f;

		[Header("Ground Setup")]
		[SerializeField] private LayerMask m_GroundLayerMask;
		[SerializeField] private float m_GroundedRadius = .2f;

		private bool canJump;
		private bool wantJump;
		private bool wantRoll;
		private bool wantAttack;
		private float rollingTime;

		protected Rigidbody2D currentRigidbody;
		private Vector2 currentVelocity = Vector3.zero;

		[Header("Events")]
		public UnityEvent OnLandingEvent = new UnityEvent();
		public UnityEvent OnJumpEvent = new UnityEvent();
		public UnityEvent OnRollingEvent = new UnityEvent();
		public UnityEvent OnAttackEvent = new UnityEvent();

        #region UNITY

        protected override void Awake()
		{
			base.Awake();
			currentRigidbody = GetComponent<Rigidbody2D>();
		}

		private void FixedUpdate()
		{
			if (!IsAlive())
				return;

			VerticalVelocity = currentRigidbody.velocity.y;
			if (VerticalVelocity < m_FallingTreshold)
			{
				bool wasGrounded = IsGrounded;
				IsGrounded = false;

				Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_GroundedRadius, m_GroundLayerMask);
				if (colliders.Length > 0 && Mathf.Abs(VerticalVelocity) < m_FallingTreshold)
					IsGrounded = true;

				if (IsGrounded)
				{
					canJump = true;
					if (IsGrounded != wasGrounded)
						OnLandingEvent.Invoke();
				}
			}

			if (!IsGrounded && m_OneJumpAllowed)
			{
				canJump = false;
			}

			if (IsRolling)
			{
				Vector2 targetVelocity = new Vector2(PrimaryDirection * m_RollingSpeed, currentRigidbody.velocity.y);
				currentRigidbody.velocity = Vector2.SmoothDamp(currentRigidbody.velocity, targetVelocity, ref currentVelocity, m_MovementSmoothing);
			}
			else if (IsGrounded)
			{
				Vector2 targetVelocity = new Vector2(HorizontalMovement * m_RunSpeed, 0);
				currentRigidbody.velocity = Vector2.SmoothDamp(currentRigidbody.velocity, targetVelocity, ref currentVelocity, m_MovementSmoothing);
			}
			else if (m_AirControl)
			{
				Vector2 targetVelocity = new Vector2(HorizontalMovement * m_RunSpeed, currentRigidbody.velocity.y);
				currentRigidbody.velocity = Vector2.SmoothDamp(currentRigidbody.velocity, targetVelocity, ref currentVelocity, m_AirSmoothing);
			}


			if (wantRoll)
			{
				if (!IsRolling && !IsAttack && IsGrounded && rollingTime < -m_RollingCooldown)
				{
					OnRollingEvent.Invoke();
					IsRolling = true;
					rollingTime = m_RollingDuration;
				}
				wantRoll = false;
			}
			else if (wantJump)
			{
				if (!IsRolling && !IsAttack && canJump && VerticalVelocity < m_FallingTreshold)
				{
					OnJumpEvent.Invoke();
					if (m_OneJumpAllowed || !IsGrounded)
						canJump = false;
					IsGrounded = false;
					currentRigidbody.velocity = new Vector2(currentRigidbody.velocity.x, m_JumpForce);
				}
				wantJump = false;
			}
			else if (wantAttack)
			{
				if (!IsRolling && m_Weapon != null && m_Weapon.CanAttack)
				{
					OnAttackEvent.Invoke();
					m_Weapon.Triggered(this);
				}
				wantAttack = false;
			}

			rollingTime -= Time.fixedDeltaTime;
			if (IsRolling && rollingTime < 0)
			{
				IsRolling = false;
			}
		}
#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			if (Selection.Contains(gameObject))
			{
				Handles.color = Color.yellow;
				Handles.DrawWireDisc(transform.position, Vector3.back, m_GroundedRadius);
			}
		}
#endif

#endregion

		public void ReleaseJump()
		{
			if (!IsAlive())
				return;

			if (m_OneJumpAllowed || !IsGrounded)
				canJump = false;
			IsGrounded = false;
			currentRigidbody.velocity = new Vector2(currentRigidbody.velocity.x, m_JumpForce);
		}

		public void WantJump()
		{
			wantJump = true;
		}

		public void WantRoll()
		{
			wantRoll = true;
		}

		public void WantMove(float move)
		{
			HorizontalMovement = move;
			if (Mathf.Abs(HorizontalMovement) > m_RunTreshold)
				PrimaryDirection = Mathf.Sign(HorizontalMovement);
		}
		public void WantLook(float direction)
		{
			PrimaryDirection = Mathf.Sign(direction);
		}

		public void WantAttack()
		{
			wantAttack = true;
		}

		public override void DirectDamage(float damage, Vector2 force)
		{
			base.DirectDamage(damage, force);
			if (IsAlive())
				currentRigidbody.velocity = Vector2.Lerp(currentRigidbody.velocity, force, 0.5f);
            else
				currentRigidbody.velocity = new Vector2(0, currentRigidbody.velocity.y);
		}

		public override void OnPause(bool pause)
		{
			if (pause)
				currentRigidbody.Sleep();
			else
				currentRigidbody.WakeUp();
		}
	}
}