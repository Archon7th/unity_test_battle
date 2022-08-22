using Assets.Scripts.GameMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemySmartAI : MonoBehaviour, IPausable
	{
		[SerializeField] private CharacterController2D m_controller;

		private float memoryMove = 0f;
		public float memoryVelocity = 0f; 
		private Waypoint waypoint;

		private void FixedUpdate()
		{
			PlayerCharacter player = PlayerCharacter.Instance;

			if (waypoint == null)
				waypoint = WaypointService.Instance.GetClosestWaypoint(transform.position);

			if (!m_controller.IsAlive() || !enabled || player == null || waypoint == null)
				return;

			Vector2 playerDelta = (player.transform.position - transform.position);
			Vector2 waypointDelta = (waypoint.transform.position - transform.position);

			if (Mathf.Abs(playerDelta.x) < 5f && playerDelta.y > -0.5f && playerDelta.y < 1f)
			{
				if (m_controller.IsAfterDamage)
					m_controller.WantMove(0);
				else if (Mathf.Abs(playerDelta.x) < 1 && Mathf.Abs(playerDelta.y) < 1)
				{
					if (!m_controller.IsAttack)
						m_controller.WantAttack();

					if (m_controller.IsAttack)
						m_controller.WantLook(Mathf.Sign(playerDelta.x));
					else
						m_controller.WantMove(Mathf.Sign(playerDelta.x));
				}
				else
					m_controller.WantMove(Mathf.Sign(playerDelta.x));
			}
			else if(waypoint.StepsCount > 0)
			{
				if (m_controller.IsAfterDamage || m_controller.IsAttack)
					m_controller.WantMove(0);
				else if (waypointDelta.y > 1f)
				{
					m_controller.WantMove(Mathf.Sign(waypointDelta.x));
					if (m_controller.IsGrounded)
						m_controller.WantJump();
				}
				else if (Mathf.Abs(waypointDelta.x) > 0.25f)
					m_controller.WantMove(Mathf.Sign(waypointDelta.x));
				else
				{
					waypoint = waypoint.GetNextWaypoint();
					// print($"waypoint {waypoint} {waypoint.StepsCount} {Mathf.Abs(waypointDelta.x)}");
				}
			}
			else
			{
				if (Mathf.Abs(memoryMove) > 0.1)
					memoryMove = Mathf.SmoothDamp(memoryMove, 0, ref memoryVelocity, 2);
				else
					memoryMove = Mathf.Sign(playerDelta.x);
				m_controller.WantMove(Mathf.Ceil(Mathf.Clamp(memoryMove, -1, 1)));
			}
		}

		public void SetWaypoint(Waypoint waypoint)
		{
			this.waypoint = waypoint;
		}

		public void OnPause(bool pause)
		{
			enabled = !pause;
		}
	}
}