using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [Header("Agents and Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private Guard[] agents;
    
    [Header("Colours")]
    [SerializeField] private Color waypointColor;
    [SerializeField] private Color agentColor;
    
    [Space]
    
    [SerializeField, Tooltip("max distance before going to the next waypoint")] private float distance;
    [SerializeField, Tooltip("DO NOT TICK IN PLAYMODE (enables waypoint creation in editor)")] private bool debug = false;

    void Awake() => SetNextWaypoint();
    void Update() => SetNextWaypoint();
    private void SetNextWaypoint() // from agent index output the transform
    {
        // loops through the agents in the array and gets the index
        // sets the destination of the agent and checks the distance between
        // from the agent and the current waypoint
        foreach (var t in agents)
        {
            var script = t.GetComponent<Guard>();                                               // gets the agents index
            if(t.gameObject.activeSelf) t.currentWaypoint = waypoints[script.waypointIndex];     // sets the destination to the current waypoint
            
            // when the agent reaches the current waypoint
            // change to the next waypoint in the list
            if(Vector3.Distance(t.transform.position, waypoints[script.waypointIndex].position) < distance)
            {
                // if the agent reaches the end of the count it will then start going backwards down the list
                script.waypointIndex += t.count;
                switch (script.waypointIndex >= waypoints.Count - 1)
                {
                    case true:
                        t.count = -1;
                        //break;
                        break;
                    default:
                    {
                        if (script.waypointIndex <= 0)
                        {
                            t.count = 1;
                        }

                        break;
                    }
                }
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        // if in debug mode
        if (debug)
        {  
            // will clear all waypoints
            waypoints.Clear();
            // then grab all waypoints that are a child of the waypoint manager
            foreach (Transform child in transform)
            {
                // and adds them to the list making sure not to add duplicates
                if(!waypoints.Contains(child))
                    waypoints.Add(child);
            }
        }
        
        // returns if no waypoints exist as to stop errors
        //if (waypoints.Count >= 2)
        // loops through all waypoints 
        for (int i = 1; i < waypoints.Count; i++)
        {
            // adds lines between each waypoint to display the path
            Debug.DrawLine(waypoints[i - 1].position, waypoints[i].position, waypointColor, 1);
        }
        
        // the same as the waypoints but for the agents instead
        if (agents == null) return;
        foreach (var t in agents)
        {
            var script = t;//.GetComponent<Guard>();
            Debug.DrawLine(t.transform.position,
                waypoints[script.waypointIndex].position, agentColor, 1);
        }
    }
}
