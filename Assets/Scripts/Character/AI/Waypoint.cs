using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Assets.Scripts.GameBehaviors
{
    public class Waypoint : MonoBehaviour
    {
        [SerializeField] protected List<Waypoint> m_pathes = new List<Waypoint>();

        public int StepsCount { get; protected set; } = 0;
        public float StepsLength { get; protected set; } = 0;

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
            int bestSteps = 0;
            Waypoint bestMatched = null;
            foreach (Waypoint el in m_pathes)
            {
                if (el != null)
                    if (bestMatched == null || el.StepsCount < bestSteps || (el.StepsCount == bestSteps && el.StepsLength < bestMatched.StepsLength))
                    {
                        bestMatched = el;
                        bestSteps = el.StepsCount;
                    }
            }
            return bestMatched;
            //return m_pathes[Random.Range(0, m_pathes.Count)];
        }

        public List<Waypoint> Track(List<Waypoint> subPathes, int steps, float length)
        {
            StepsCount = steps;
            StepsLength = length;
            foreach (Waypoint el in m_pathes)
                if (el != null && el.StepsCount < 0)
                {
                    el.StepsCount = steps + 1;
                    el.StepsLength = length + Vector3.Distance(transform.position, el.transform.position);
                    subPathes.Add(el);
                }

            return subPathes;
        }

        public void Reset()
        {
            StepsCount = -1;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Selection.Contains(gameObject) || Selection.Contains(transform.parent.gameObject))
            {
                Handles.color = Color.cyan;

                Handles.Label(transform.position,"T:"+StepsLength);
                Handles.DrawWireDisc(transform.position, Vector3.back, 0.1f);
                foreach (Waypoint el in m_pathes)
                    if (el != null)
                    {
                        Handles.DrawWireDisc(el.transform.position, Vector3.back, 0.05f);
                        Handles.DrawLine(transform.position, Vector3.Lerp(transform.position, el.transform.position, 0.5f));
                    }
                        
            }
        }
#endif
    }
}