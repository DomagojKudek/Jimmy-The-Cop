using UnityEngine;
using System.Collections;
using Panda;
namespace Assets.Scripts.Enemy
{
    public class Patroller1 : MonoBehaviour
    {
        public WaypointPath1 waypointPath;

        Unit1 self;
        public BatControllerBoss controller;
        int waypointIndex;

        // Use this for initialization
        void Start()
        {
            waypointIndex = 0;
            self = GetComponent<Unit1>();
            controller = GetComponent<BatControllerBoss>();
        }



        [Task]
        bool NextWaypoint()
        {
            if (waypointPath != null)
            {
                if(controller.currentPayload!=null&& waypointIndex==0){
                   waypointIndex++;
                }
                waypointIndex++;
                if( Task.isInspected )
                    Task.current.debugInfo = string.Format("i={0}", waypointArrayIndex);
            }
            return true;
        }

        [Task]
        bool SetDestination_Waypoint()
        {
            bool isSet = false;
            if (waypointPath != null)
            {
                var i = waypointArrayIndex;
                var p = waypointPath.waypoints[i].position;
               //isSet = self.SetDestination(p);
               isSet=controller.SetDestination(p);
            }
            return isSet;
        }

        [Task]
        public bool SetDestination_Waypoint(int i)
        {
            bool isSet = false;
            if (waypointPath != null)
            {
                var p = waypointPath.waypoints[i].position;
                isSet = self.SetDestination(p);
            }
            return isSet;
        }

        [Task]
        public void MoveTo(int i)
        {
            SetDestination_Waypoint(i);
            self.MoveTo_Destination();
            self.WaitArrival();
        }

        [Task]
        public void LookAt(int i)
        {
            self.SetTarget( waypointPath.waypoints[i].transform.position);
            self.AimAt_Target();
        }


        int waypointArrayIndex
        {
            get
            {
                int i = 0;
                if(controller.currentPayload==null||controller.currentPayload.transform.parent==null){
                    if(Random.Range(0,1.0f)<0.7){
                        return 0;
                    }else{
                        return 1;
                    }
                }
                if( waypointPath.loop)
                {
                    i = waypointIndex % waypointPath.waypoints.Count;
                }
                else
                {
                    int n = waypointPath.waypoints.Count;
                    i = waypointIndex % (n*2);

                    if( i > n-1 )
                        i = (n-1) - (i % n); 
                }

                return i;
            }
        }
    }

}