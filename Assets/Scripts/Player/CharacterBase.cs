using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game;

public class CharacterBase : MonoBehaviour, IDamageReciever, IDamageDealer, IItemUser, IPausable
{
	[SerializeField] protected WeaponBase m_Weapon;
	[SerializeField] protected float m_WeponPower = 1;
	[SerializeField] protected float m_MaxHealth = 20;
	public float Health { get; protected set; }


	public bool IsRolling { get; protected set; } = false;
	public bool IsGrounded { get; protected set; } = false;
	public float HorizontalMovement { get; protected set; }
	public float VerticalVelocity { get; protected set; }

	public UnityEvent OnDeathEvent;

	protected float primaryDirection;
	protected Rigidbody2D currentRigidbody;


	public CharacterBase()
    {
		Health = m_MaxHealth;

		if (OnDeathEvent == null)
			OnDeathEvent = new UnityEvent();
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
			{
				OnDeathEvent.Invoke();
				GameController.ShowDeathMenu();
			}
		}
	}

	public bool AcceptUse(IItemUsable item)
	{
		return true;
	}

	public bool AcceptDamageFrom(IDamageDealer source)
	{
		if (Equals(source) || !IsAlive())
			return false;

		//Damage(source.GetDamage());

		print("CharacterBase AcceptDamage "+ source);
		return true;
	}

	public void DirectDamage(float damage, Vector2 force)
	{
		currentRigidbody.velocity += force;
		Damage(damage);
	}

	public bool DealDamageTo(IDamageReciever target)
	{
		target.DirectDamage(m_WeponPower, new Vector2(primaryDirection, 1).normalized);
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
