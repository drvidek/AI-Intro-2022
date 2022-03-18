using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        berrypick,
        attack,
        run
    }

    public State currentState;

    public AIMovement aiMovement;

    public float chaseDist;

    public bool alone;

    private void Start()
    {
        aiMovement = GetComponent<AIMovement>();
        currentState = State.berrypick;
        NextState();
    }

    private void NextState()
    {
        Debug.Log("Next State");
        switch (currentState)
        {
            case State.berrypick:
                StartCoroutine(BerryState());
                break;
            case State.attack:
                StartCoroutine(AttackState());
                break;
            case State.run:
                StartCoroutine(RunState());
                break;
            
        }
    }

    private void CheckAlone()
    {
        var _enemyCount = GameObject.FindGameObjectsWithTag("Enemy");
        if (_enemyCount.Length <= 1)
            alone = true;
    }

    private IEnumerator AttackState()
    {
        Debug.Log("Attack: Enter");

        while (currentState == State.attack)
        {
            aiMovement.AIMove(aiMovement.player);
            if (Vector2.Distance(transform.position, aiMovement.player.position) > chaseDist)
            {
                CheckAlone();
                if (!alone)
                    currentState = State.berrypick;
                else
                    currentState = State.run;
            }
            yield return null;
        }
        aiMovement.FindNearestWaypoint();
        Debug.Log("Attack: Exit");
        NextState();
    }

    private IEnumerator RunState()
    {
        Debug.Log("Run: Enter");
        while (currentState == State.run)
        {
            aiMovement.AIMoveAway(aiMovement.player.transform);
            if (Vector2.Distance(transform.position, aiMovement.player.position) > chaseDist * 1.5f)
            {
                currentState = State.berrypick;
            }
            yield return null;
        }
        Debug.Log("Run: Exit");
        NextState();
    }

    private IEnumerator BerryState()
    {
        Debug.Log("Berry: Enter");

        while (currentState == State.berrypick)
        {
            aiMovement.AIMove(aiMovement.waypoints[aiMovement.waypointIndex]);
            aiMovement.WaypointUpdate();

            if (Vector2.Distance(transform.position, aiMovement.player.position) < chaseDist)
            {
                CheckAlone();
                if (!alone)
                    currentState = State.attack;
                else
                    currentState = State.run;
            }
            yield return null;
        }
        Debug.Log("Berry: Exit");
        NextState();
    }
}