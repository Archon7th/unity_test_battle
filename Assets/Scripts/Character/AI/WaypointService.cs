using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.GameBehaviors
{
    public class WaypointService : MonoBehaviour
    {
        public static WaypointService Instance { get; protected set; }

        private readonly List<Waypoint> waypoints = new List<Waypoint>();

        public WaypointService() : base()
        {
            Instance = this;
        }

        public void PlayerTouch(Waypoint waypoint)
        {
            // print($"PlayerTouch {waypoint}");
            foreach (Waypoint el in waypoints)
                el.Reset();

            List<Waypoint> subPathes = new List<Waypoint>();
            waypoint.Track(subPathes, 0, 0f);
            while (subPathes.Count > 0)
            {
                List<Waypoint> newSubPathes = new List<Waypoint>();
                foreach (Waypoint el in subPathes)
                {
                    el.Track(newSubPathes, el.StepsCount, el.StepsLength);

                }
                subPathes = newSubPathes;
            }
            
        }

        public void Register(Waypoint waypoint)
        {
            if (!waypoints.Contains(waypoint))
                waypoints.Add(waypoint);
        }

        public Waypoint GetClosestWaypoint(Vector3 position)
        {
            float bestRange = 0;
            Waypoint bestMatched = null;
            foreach (Waypoint el in waypoints)
            {
                float dist = Vector3.Distance(position, el.transform.position);
                if (bestMatched == null || dist < bestRange)
                {
                    bestMatched = el;
                    bestRange = dist;
                }
            }
            return bestMatched;
        }
    }
}