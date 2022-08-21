using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemyCharacter : CharacterController2D
	{
		[Header("Enemy")]
		[SerializeField] private GameObject m_heart;
		[SerializeField] private float m_deadTimeout = 60;


		public override bool CanDamage()
		{
			if (IsAfterDamage)
				return false;

			return base.CanDamage();
		}

		public override void KilledByDealer(IDamageDealer source)
		{
			Instantiate(m_heart, transform.position, Quaternion.identity);
			Destroy(gameObject, m_deadTimeout);
		}
	}
}