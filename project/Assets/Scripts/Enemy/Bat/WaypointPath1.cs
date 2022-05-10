using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using Panda;


namespace Assets.Scripts.Enemy
{


    public class WaypointPath1 : MonoBehaviour
    {
        public bool loop;
        public List<Transform> waypoints{
            get{
                var wp = new List<Transform>();
                foreach( Transform c in this.transform)
                {
                    wp.Add(c);
                }
                return wp;
            }
        }
        //public List<Transform> waypoints=new List<Transform>();
        public GameObject player;
        // Use this for initialization
        void Start()
        {
            player = PlayerManager.instance;
        }

        void OnDrawGizmos()
        {
            DrawLines();
            DrawSpheres(0.1f);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            DrawLines();
            DrawSpheres(0.5f);
        }

        void DrawSpheres(float r)
        {
            var wp = waypoints;
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.DrawSphere(wp[i].position, r);
                //Handles.Label(wp[i].position, "wp"+i);
            }
        }

        void DrawLines()
        {
            var wp = waypoints;
            for (int i = 1; i < waypoints.Count; i++)
            {
                var p0 = wp[i - 1].position;
                var p1 = wp[i - 0].position;
                Gizmos.DrawLine(p0, p1);
            }

            if( loop && wp.Count > 1 )
            {
                var p0 = wp[0].position;
                var p1 = wp[wp.Count-1].position;
                Gizmos.DrawLine(p0, p1);
            }
        }
    }
}
