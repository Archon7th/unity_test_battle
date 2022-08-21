using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	

	public class CharacterBase : MonoBehaviour, IDamageReciever, IDamageDealer, IPausable
	{
		[SerializeField] protected WeaponBase m_Weapon;
		[SerializeField] protected int m_SideIndex = 1;
		[SerializeField] protected float m_WeponPower = 1;
		[SerializeField] protected float m_HealthMax = 20;
		[SerializeField] protected float m_AfterDamageTime = 0.1f;
		public float Health { get; protected set; }
		public float HealthMax { get { return m_HealthMax; } }


		public bool IsRolling { get; protected set; } = false;
		public bool IsGrounded { get; protected set; } = false;
		public bool IsAttack { get { return (m_Weapon != null && m_Weapon.IsReloading); } }
		public bool IsAfterDamage { get { return (afterDamageTime > 0 && (Time.time - afterDamageTime <= m_AfterDamageTime)); } }
		public float AfterDamage { get { return (Time.time - afterDamageTime) / m_AfterDamageTime; } }
		public float HorizontalMovement { get; protected set; }
		public float PrimaryDirection { get; protected set; }
		public float VerticalVelocity { get; protected set; }

		public UnityEvent OnDeathEvent = new UnityEvent();
		public UnityEvent OnDamageEvent = new UnityEvent();
		public UnityEvent<float> OnHealthEvent = new UnityFloatEvent();


		protected float afterDamageTime;
		protected Rigidbody2D currentRigidbody;


		private void Awake()
		{
			Health = m_HealthMax;
			OnHealthEvent.Invoke(Health);
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
				OnHealthEvent.Invoke(-damage);
				if (!IsAlive())
				{
					OnDeathEvent.Invoke();
					currentRigidbody.velocity = new Vector2(0, currentRigidbody.velocity.y);
					enabled = false;
				}
			}
		}

		public bool CanRecieveHeal()
		{
			return (IsAlive() && Health < HealthMax);
		}

		public void Heal(float heal)
		{
			if (IsAlive())
			{
				Health += heal;
				OnHealthEvent.Invoke(heal);
				if (Health > HealthMax)
					Health += HealthMax;
			}
		}

		#region DAMAGE_INTERFACES

		public virtual bool AcceptUseItem(IItemUsable item)
		{
			return false;
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

		public virtual bool CanDamage()
		{
			return IsAlive();
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
			currentRigidbody.velocity = Vector2.Lerp(currentRigidbody.velocity, force, 0.5f);
			afterDamageTime = Time.time;
			Damage(damage);
		}

		public bool DealDamageFromInto(IDamageDealer source, IDamageReciever target)
		{
			Vector2 delta = target.GetDamagePositioin() - GetDamagePositioin();
			delta.x = Mathf.Sign(delta.x);
			delta.y = Mathf.Clamp(delta.x, -1, 1);
			target.DirectDamage(source.GetDamage(), 10f * (delta.normalized + new Vector2(PrimaryDirection, 1).normalized));
			if (!target.IsAlive())
            {
				KillsTarget(target);
				target.KilledByDealer(this);
			}
				
			return true;
		}

        protected virtual void KillsTarget(IDamageReciever target)
        {

        }
		
		public virtual void KilledByDealer(IDamageDealer source)
		{

		}

		#endregion

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
