using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemyCharacter : CharacterController2D
	{
		[Header("Enemy")]
		[SerializeField] private GameObject m_heart;
		[SerializeField] private TargetBlinkTweak m_blinkTweak;
		[SerializeField] private float m_spawnTimeout = 1;
		[SerializeField] private float m_deadTimeout = 60;

		public override bool CanDamage()
		{
			if (IsAfterDamage)
				return false;

			return base.CanDamage();
		}

		public override void KilledByDealer(IDamageDealer source)
		{
			Invoke(nameof(SpawnLoot), m_spawnTimeout);
			if (m_blinkTweak)
				m_blinkTweak.SetDelayTimeToEnd(m_deadTimeout);
			Destroy(gameObject, m_deadTimeout);
		}

		public void SehHealthOnSpawn(float value)
		{
			m_HealthMax = value;
			Health = value;
		}

		private void SpawnLoot()
		{
			if (m_heart)
				Instantiate(m_heart, transform.position, Quaternion.identity);
		}
	}
}