using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public Transform player;
    public float chaseDist;

    public List<Transform> waypoints;
    public int waypointIndex = 0;
    public GameObject waypointPrefab;

    public float speed = 1.5f;
    public float minGoalDist = 0.1f;

    private bool chasing = false;

    public SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        FindNearestWaypoint();
        ShuffleWaypoints();
    }

    public void AIMove(Transform goal)
    {
        Vector2 AIPosition = transform.position;
        Vector2 dirToGoal = goal.position - transform.position;
        SetSpriteDir(dirToGoal.x);

        //if we're not near the goal
        if (Vector2.Distance(AIPosition, goal.position) > minGoalDist)
        {

            dirToGoal.Normalize();
            transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;

        }
        else
        {
            if (goal != player)
                transform.position = goal.position;
        }

        if (chasing == false && goal == player)
        {
            chasing = true;
        }
    }

    public void AIMoveAway(Transform goal)
    {
        Vector2 dirToGoal = transform.position - goal.position;
        SetSpriteDir(dirToGoal.x);
        dirToGoal.Normalize();
        transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;

        chasing = false;
    }

    public void SetSpriteDir(float xdir)        ///this straight up wasn't working inside AIMove() so it's a method instead
    {
        if (xdir > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void FindNearestWaypoint()
    {
        int closePoint = 0;
        float dist = float.PositiveInfinity;
        float tempDist;
        for (int i = 0; i < waypoints.Count; i++)
        {
            tempDist = Vector2.Distance(transform.position, waypoints[i].position);
            //check if the distance to point i is smaller than point i-1
            if (tempDist < dist)
            {
                //if it is, set the new minimum distance to that distance, and save which waypoint index it was
                dist = tempDist;
                closePoint = i;
            }
        }
        //then set the waypoint index to the point determined
        waypointIndex = closePoint;
    }

    public void WaypointUpdate()        //update which waypoint we are moving towards
    {
        //once we reach the goal
        if (transform.position == waypoints[waypointIndex].position)
        {
            //increment the waypoint index and wrap back to zero if we surpass maximum
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }

    public void ShuffleWaypoints()      //
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform temp = waypoints[i];  //save the current entry in a variable
            int randomIndex = Random.Range(i, waypoints.Count); //choose a random number between the current entry and the list maximum

            //swaps the two points
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }
}
