using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine : MonoBehaviour
{
    public enum State
    {
        berrypick,
        attack,
        run
    }

    public State currentState;  //store my current state
    public float chaseDist; //minimum distance before I chase/run from the player
    public float runDist;   //minimum distance before I stop running from the player
    public bool alone;  //stores if I am the last enemy left
    public AIMovement aiMovement;   //my AIMovement component
    public Image icon;  //my state icon
    public Sprite[] iconList;   //my possible state icons

    private void Start()
    {
        //get my AIMovement component
        aiMovement = GetComponent<AIMovement>();
        //Set my state to default
        currentState = State.berrypick;
        //Trigger the state machine
        NextState();
    }

    private void NextState()
    {
        Debug.Log("Next State");
        //complete the appropriate co-routine based on my state
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

        //update my icon to match my current state
        icon.sprite = iconList[(int)currentState];

    }

    private void CheckAlone()
    {
        var _enemyCount = GameObject.FindGameObjectsWithTag("Enemy");   //create an array storing each game object tagged as "Enemy"

        //if there is only one enemy left (i.e. myself), I am alone
        if (_enemyCount.Length <= 1)
            alone = true;
    }

    #region States

    private IEnumerator AttackState()
    {
        Debug.Log("Attack: Enter");

        //while i'm in the attack state
        while (currentState == State.attack)
        {
            CheckAlone();
            //if I'm alone, move into the Run state
            if (alone)
            {
                currentState = State.run;
            }
            else
            {
                //move towards the player
                aiMovement.AIMove(aiMovement.player);

                //if I get too far away from the player, return to berrypicking state
                if (Vector2.Distance(transform.position, aiMovement.player.position) > chaseDist)
                {
                    currentState = State.berrypick;
                }
            }
            //wait for a frame before looping
            yield return null;
        }

        //update my current waypoint to the nearest when exiting the loop
        aiMovement.FindNearestWaypoint();
        Debug.Log("Attack: Exit");

        //change behaviours based on state
        NextState();
    }

    private IEnumerator RunState()
    {
        Debug.Log("Run: Enter");
        //while i'm in the Run state
        while (currentState == State.run)
        {
            //move away from the player
            aiMovement.AIMoveAway(aiMovement.player.transform);
            //if I get far away enough from the player, go back to berrypicking
            if (Vector2.Distance(transform.position, aiMovement.player.position) > runDist)
            {
                currentState = State.berrypick;
            }
            //wait a frame before looping
            yield return null;
        }
        Debug.Log("Run: Exit");
        //change behaviours based on state
        NextState();
    }

    private IEnumerator BerryState()
    {
        Debug.Log("Berry: Enter");
        //when I'm in the Berrypicking state
        while (currentState == State.berrypick)
        {
            //move towards my current waypoint
            aiMovement.AIMove(aiMovement.waypoints[aiMovement.waypointIndex]);
            //
            aiMovement.WaypointUpdate();

            //if I get close enough to the player
            if (Vector2.Distance(transform.position, aiMovement.player.position) < chaseDist)
            {
                //if I'm not alone, go into Attack state - otherwise, go to Run state
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

    #endregion
}