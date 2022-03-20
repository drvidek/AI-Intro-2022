using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Movement stats")]
    public float speed = 1.5f;  //movement speed
    public float minGoalDist = 0.1f;    //distance before I jump to match the waypoint position

    [Header("Waypoint details")]
    public Transform player;    //the player transform
    public List<Transform> waypoints;   //a list of the waypoints I can use
    public int waypointIndex = 0;   //the current waypoint I'm heading to

    [Header("Sprite")]
    public SpriteRenderer spriteRenderer;   //my sprite

    public void Start()
    {
        //get the sprite
        spriteRenderer = GetComponent<SpriteRenderer>();
        //shuffle the order of waypoints for variation
        ShuffleWaypoints();
        //find the nearest waypoint
        FindNearestWaypoint();

    }

    public void AIMove(Transform goal)
    {
        //store our position
        Vector2 AIPosition = transform.position;
        //get the direction of our goal point (A to B = B - A)
        Vector2 dirToGoal = goal.position - transform.position;
        //use our X dir to determine sprite direction
        SetSpriteDir(dirToGoal.x);

        //if we've not reached our minimum distance
        if (Vector2.Distance(AIPosition, goal.position) > minGoalDist)
        {
            //set our vector to a length of one unit
            dirToGoal.Normalize();
            //multiply by our speed, account for framerate differences with deltatime, and add the vector to our current position to move us
            transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;
        }
        else    //if we have reached out minimum distance from a goal
        {
            //if you're not heading towards the player, snap to the position of the waypoint
            if (goal != player)
                transform.position = goal.position;
        }
    }

    public void AIMoveAway(Transform goal)
    {
        //get the direction away from the goal (the player)
        Vector2 dirToGoal = transform.position - goal.position;
        //set your sprite direction
        SetSpriteDir(dirToGoal.x);
        //set the vector length to one unit
        dirToGoal.Normalize();
        //move accounting for speed and deltatime
        transform.position += (Vector3)dirToGoal * speed * Time.deltaTime;
    }

    public void SetSpriteDir(float xdir)        ///this straight up wasn't working inside AIMove() so it's a method instead
    {
        if (xdir > 0)   //if we are moving right
        {
            spriteRenderer.flipX = false;   //point our sprite right
        }
        else
        {
            spriteRenderer.flipX = true;    //point our sprite left
        }
    }

    public void FindNearestWaypoint()   //find which waypoint is closest to me
    {
        int closePoint = 0; //for the index of the nearest point
        float dist = float.PositiveInfinity;    //for the distance to the nearest point
        float tempDist; //for the distance to the current index point

        //repeat for the total number of waypoints
        for (int i = 0; i < waypoints.Count; i++)
        {
            //store the distance between us and waypoint i
            tempDist = Vector2.Distance(transform.position, waypoints[i].position);

            //check if the distance to point i is closer than the currently stored closest distance
            if (tempDist < dist)
            {
                //if it is, set the minimum distance to this new distance, and save which waypoint index it was
                dist = tempDist;
                closePoint = i;
            }
        }
        //then set the waypoint index to the point determined as closest
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

    public void ShuffleWaypoints()      //randomise the order of the waypoints array
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            Transform temp = waypoints[i];  //save the current entry in a variable
            int randomIndex = Random.Range(i, waypoints.Count); //choose a random number between the current entry and the list maximum

            //swap the two points
            waypoints[i] = waypoints[randomIndex];
            waypoints[randomIndex] = temp;
        }
    }
}
