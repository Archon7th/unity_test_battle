using Assets.Scripts.GameMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemyDumbAI : MonoBehaviour, IPausable
	{
		[SerializeField] private CharacterController2D m_controller;

		private void FixedUpdate()
		{
			PlayerCharacter player = PlayerCharacter.Instance;

			if (!m_controller.IsAlive() || !enabled || player == null)
				return;

			Vector2 delata = (player.transform.position - transform.position);
			if (Mathf.Abs(delata.x) > 1f)
			{
				if (!m_controller.IsAfterDamage)
					m_controller.WantMove(Mathf.Sign(delata.x));
				else
					m_controller.WantMove(0);
			}
			else if (!m_controller.IsAttack)
			{
				if (Random.Range(0, 1f) > 0.5f)
					m_controller.WantMove(Mathf.Sign(delata.x));
				else
					m_controller.WantMove(0);

				if (!m_controller.IsAfterDamage && Mathf.Abs(delata.y) < 1f)
					m_controller.WantAttack();
			}
			else
			{
				m_controller.WantLook(Mathf.Sign(delata.x));
			}

			if ((m_controller.IsGrounded || m_controller.IsAfterDamage) && !m_controller.IsAttack && delata.y > 1f && Mathf.Abs(delata.x) > 1f)
				m_controller.WantJump();
		}

		public void OnPause(bool pause)
		{
			enabled = !pause;
		}
	}
}