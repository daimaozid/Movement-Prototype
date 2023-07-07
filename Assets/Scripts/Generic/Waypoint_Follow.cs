using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint_Follow : MonoBehaviour
{
    //Instance Variables
    [SerializeField] private GameObject[] waypoints; //Waypoint array
    private int currentWaypointIndex = 0; //Current waypoint's index
    [SerializeField] private float speed = 2f; //Speed the object will move at
    private Vector3 waypointPos; //Position of current waypoint
    private Vector3 objectPos; //Position of object

    // Update is called once per frame
    private void Update()
    {

        waypointPos = waypoints[currentWaypointIndex].transform.position; //Updates position of current waypoint
        objectPos = transform.position; //Updates position of current waypoint

        //If the object reaches current waypoint, selects next waypoint as target
        //If the next waypoint doesn't exist, reset target to the first waypoint
        if (Vector2.Distance(waypointPos, objectPos) < 0.1f) {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length) {
                currentWaypointIndex = 0;
            }
        }

        //Moves the object toward the waypoint
        //Time.deltaTime helps to control the speed of object per second so it's consistent
        //In other words, object movement speed is framerate independent
        transform.position = Vector2.MoveTowards(objectPos, waypointPos, Time.deltaTime * speed);
    }
}
