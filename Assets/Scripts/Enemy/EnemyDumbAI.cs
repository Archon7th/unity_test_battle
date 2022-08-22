using Assets.Scripts.GameMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemyDumbAI : MonoBehaviour, IPausable
	{
		[SerializeField] private CharacterController2D m_controller;

		private float memoryMove = 0f;
		public float memoryVelocity = 0f;

		private void FixedUpdate()
		{
			PlayerCharacter player = PlayerCharacter.Instance;

			if (!m_controller.IsAlive() || !enabled || player == null)
				return;

			Vector2 delata = (player.transform.position - transform.position);
			if (Mathf.Abs(delata.x) > 1f)
			{
				if (m_controller.IsAfterDamage)
					m_controller.WantMove(0);
				else if (Mathf.Abs(delata.y) > 1)
				{
					if (Mathf.Abs(memoryMove) > 0.1)
						memoryMove = Mathf.SmoothDamp(memoryMove, 0, ref memoryVelocity, 2);
					else
						memoryMove = (Random.Range(0f, Mathf.Abs(0.5f * delata.x)) > 0.5) ? Mathf.Sign(delata.x) : 0;
					m_controller.WantMove(Mathf.Ceil(Mathf.Clamp(memoryMove, -1, 1)));
				}
				else
					m_controller.WantMove(Mathf.Sign(delata.x));
			}
			else
			{
				if (Mathf.Abs(delata.y) < 1f) {
					if (!m_controller.IsAfterDamage && !m_controller.IsAttack)
					{
						memoryMove = (Random.Range(0f, 1f) > 0.5) ? delata.x : 0;
						m_controller.WantAttack();
					}

					if (m_controller.IsAttack)
						m_controller.WantLook(Mathf.Sign(delata.x));
					else
						m_controller.WantMove(Mathf.Round(Mathf.Clamp(memoryMove, -1, 1)));
				}
				else
                {
					if (!m_controller.IsAfterDamage)
                    {
						if (Mathf.Abs(memoryMove) > 0.1)
							memoryMove = Mathf.SmoothDamp(memoryMove, 0, ref memoryVelocity, 2);
						else
							memoryMove = Mathf.Sign(delata.x);
						m_controller.WantMove(Mathf.Ceil(Mathf.Clamp(memoryMove, -1, 1)));
					}
				}
			}

			if ((m_controller.IsGrounded || m_controller.IsAfterDamage) && !m_controller.IsAttack)
				if (delata.y > Mathf.Min(1f, Mathf.Abs(0.5f * delata.x)) && Mathf.Abs(delata.x) > 1f)
					m_controller.WantJump();
		}

		public void OnPause(bool pause)
		{
			enabled = !pause;
		}
	}
}