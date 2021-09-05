using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AgentStates
{
    Patrol,
    Search,
    Chase
}

public delegate void StateDelegate();
[RequireComponent(typeof(Guard))]
public class StateMachine : MonoBehaviour
{
    private void Awake() => guard = GetComponent<Guard>();
    private Guard guard;

    private Dictionary<AgentStates, StateDelegate> states = new Dictionary<AgentStates, StateDelegate>();
    [SerializeField] private AgentStates currentState = AgentStates.Patrol;
    public void ChangeState(AgentStates _newState) => currentState = _newState;
    
    private void Start()
    {
        states.Add(AgentStates.Patrol, delegate { guard.AgentDestination(guard.currentWaypoint); });
        states.Add(AgentStates.Chase, delegate { guard.AgentDestination(guard.player); });
        states.Add(AgentStates.Search, delegate { if(guard.search)StartCoroutine(Search()); });
    }

    private void Update()
    {
        // These two lines are used to run the state machine
        // it works by attempting to retrieve the relevant function for the current state.
        // then running the function if it successfully found it 
        if(states.TryGetValue(currentState, out StateDelegate state)) state.Invoke();
        else Debug.Log($"No State Was Set For {currentState}.");
        
        var scanArea = guard.currentWaypoint.GetComponent<WaypointOptions>();
        if(scanArea.isScannable && Vector3.Distance(guard.transform.position, guard.currentWaypoint.position) <= 0.5f && currentState != AgentStates.Search)
        {
            guard.search = true;
            currentState = AgentStates.Search;
        }
        else if(guard.targetSpotted)
        {
            currentState = AgentStates.Chase;
            guard.agent.speed = 5;
        }
        if(!guard.targetSpotted && currentState == AgentStates.Chase)
            StartCoroutine(WaitTime());
    }

    IEnumerator Search()
    {
        guard.search = false;
        yield return new WaitForSeconds(5);
        
        if(currentState != AgentStates.Chase) 
            currentState = AgentStates.Patrol;
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(5);
        if(currentState == AgentStates.Chase && 
           !guard.targetSpotted) currentState = AgentStates.Patrol;
    }
}
