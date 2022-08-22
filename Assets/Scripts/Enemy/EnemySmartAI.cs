using Assets.Scripts.GameMenu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameBehaviors
{
	public class EnemySmartAI : MonoBehaviour, IPausable
	{
		[SerializeField] private CharacterController2D m_controller;

		private Waypoint waypoint;

		private void FixedUpdate()
		{
			PlayerCharacter player = PlayerCharacter.Instance;

			if (waypoint == null) {
				waypoint = WaypointService.Instance.GetClosestWaypoint(transform.position);
				print("waypoint " + waypoint);
			}

			if (!m_controller.IsAlive() || !enabled || player == null || waypoint == null)
				return;

			Vector2 playerDelta = (player.transform.position - transform.position);
			Vector2 waypointDelta = (waypoint.transform.position - transform.position);

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