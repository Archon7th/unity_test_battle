using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameBehaviors
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] private List<Waypoint> m_pathes = new List<Waypoint>();
        protected int steps;


        private void Awake()
        {
            WaypointService.Instance.Register(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
            if (player)
                WaypointService.Instance.PlayerTouch(this);

            EnemySmartAI enemy = collision.gameObject.GetComponent<EnemySmartAI>();
            if (enemy)
                enemy.SetWaypoint(GetNextWaypoint());
        }

        public Waypoint GetNextWaypoint()
        {
            print($"GetNextWaypoint {steps}");
            return m_pathes[Random.Range(0, m_pathes.Count)];
        }

        public void Track(int steps)
        {
            this.steps = steps;
            foreach (Waypoint el in m_pathes)
                if (el.steps < 0)
                    el.Track(steps + 1);
        }

        public void Reset()
        {
            foreach (Waypoint el in m_pathes)
                el.steps = -1;
        }
    }
}