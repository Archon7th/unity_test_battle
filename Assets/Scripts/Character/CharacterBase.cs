using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	public class CharacterBase : MonoBehaviour, IDamageReciever, IDamageDealer, IItemUser, IPausable
	{
		[SerializeField] protected WeaponBase m_Weapon;
		[SerializeField] protected int m_SideIndex = 1;
		[SerializeField] protected float m_WeponPower = 1;
		[SerializeField] protected float m_MaxHealth = 20;
		[SerializeField] protected float m_AfterDamageTime = 0.1f;
		public float Health { get; protected set; }


		public bool IsRolling { get; protected set; } = false;
		public bool IsGrounded { get; protected set; } = false;
		public bool IsAttack { get { return (m_Weapon != null && m_Weapon.IsReloading); } }
		public bool IsAfterDamage { get { return (Time.time - afterDamageTime <= m_AfterDamageTime); } }
		public float HorizontalMovement { get; protected set; }
		public float PrimaryDirection { get; protected set; }
		public float VerticalVelocity { get; protected set; }

		public UnityEvent OnDeathEvent = new UnityEvent();
		public UnityEvent OnDamageEvent = new UnityEvent();


		protected float afterDamageTime;
		protected Rigidbody2D currentRigidbody;


		public CharacterBase()
		{
			Health = m_MaxHealth;
		}

		private void Awake()
		{
			currentRigidbody = GetComponent<Rigidbody2D>();
		}

		public bool IsAlive()
		{
			return (Health > 0);
		}

		public void Damage(float damage)
		{
			if (IsAlive())
			{
				Health -= damage;
				if (!IsAlive())
					OnDeathEvent.Invoke();
			}
		}

		public bool AcceptUse(IItemUsable item)
		{
			return true;
		}

		public int GetSideIndex()
		{
			return m_SideIndex;
		}

		public Vector2 GetDamagePositioin()
        {
			return transform.position;
        }

		public float GetDamage()
		{
			return m_WeponPower;
		}

		public bool CanDamage()
		{
			return (IsAlive() && !IsRolling && !IsAttack && !IsAfterDamage);
		}

		public bool AcceptDamageFrom(IDamageDealer source)
		{
			if (Equals(source) || !IsAlive() || GetSideIndex() == source.GetSideIndex() || IsRolling || IsAfterDamage)
				return false;

			//Damage(source.GetDamage());

			print($"CharacterBase {name} AcceptDamage {source}");
			return true;
		}

		public void DirectDamage(float damage, Vector2 force)
		{
			OnDamageEvent.Invoke();
			currentRigidbody.velocity += force;
			afterDamageTime = Time.time;
			Damage(damage);
		}

		public bool DealDamageFromInto(IDamageDealer source, IDamageReciever target)
		{
			Vector2 delta = (GetDamagePositioin() - source.GetDamagePositioin());
			target.DirectDamage(source.GetDamage(), delta.normalized * 2 + new Vector2(PrimaryDirection, 1).normalized);
			return true;
		}


		public void OnPause(bool pause)
		{
			enabled = !pause;
			if (pause)
				currentRigidbody.Sleep();
			else
				currentRigidbody.WakeUp();
		}
	}
}
