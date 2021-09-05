using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
	[Header("Sight Debugging")]
	[SerializeField] private float lineDist;
	[SerializeField] private float lineOffsetDist;
	
	[Header("Waypoints")]
	[Min(0)]public int waypointIndex = 0;
	public int count;
	public Transform currentWaypoint;
	
	[Header("Search and Targetting")]
	public Transform player;
	public bool search = false;
	public bool targetSpotted;

	[NonSerialized] public float angle;
	[NonSerialized] public NavMeshAgent agent;

	private void Awake()
	 {
		player = FindObjectOfType<PlayerController>().transform; 
		agent = GetComponent<NavMeshAgent>();
	 }
	private void Update() => AgentSight();

	public void AgentDestination(Transform _target)
	{
		agent.destination = _target.position;
		agent.speed = 2.5f;
	}
	/// <summary> Handles looking for the player and only chasing if there is clear line of sight </summary>
	private void AgentSight()
	{
		var dir = player.position - transform.position;
		var sightAngle = Vector3.Angle(dir, transform.forward);
		this.angle = sightAngle;
        
		//13.63 50.12032
		// if the angle is less than or equal to 50 then see if there is line of sight
		if (sightAngle <= 50.12032f)
		{
			RaycastHit2D hit = (Physics2D.Linecast(transform.position, player.position));
			Debug.DrawLine(transform.position, player.position, Color.green);
			if (hit.collider.CompareTag("Player") && !player.GetComponent<PlayerController>().lookMarker.hidden)
			{
				Debug.DrawLine(transform.position, player.position, Color.green);
				targetSpotted = true;
			}
			else if(player.GetComponent<PlayerController>().lookMarker.hidden)
			{
				Debug.DrawLine(transform.position, player.position, Color.red);
				targetSpotted = false;
			}
			else
			{
				Debug.DrawLine(transform.position, player.position, Color.red);
				targetSpotted = false;
			}
		}
		else targetSpotted = false;
	}
	
	/// <summary> Used to debug the agents sight lines </summary>
	private void Debugging()
	{
		float distance = lineDist;                         // used to make it go further out
		float offsetDistance = lineOffsetDist;             // used to spread out the lines
		Vector3 offset = transform.right * offsetDistance; // sets the offset
		Vector3 forward = transform.forward * distance;    // sets the direction

		Debug.DrawLine(transform.position, transform.position + forward - offset, Color.blue); // left
		Debug.DrawLine(transform.position, transform.position + forward, Color.blue);          // middle
		Debug.DrawLine(transform.position, transform.position + forward + offset, Color.blue); // right
	}

	private void OnDrawGizmos() => Debugging();
}
