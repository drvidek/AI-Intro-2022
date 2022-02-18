using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{

    public Transform player;
    public float chaseDist;

    public GameObject[] waypoints;
    public int waypointIndex = 0;
    private int lastPoint = 0;

    public float speed = 1.5f;
    public float minGoalDist = 0.1f;

    private bool chasing = false;

    // Update is called once per frame
    void Update()
    {
        Vector2 AIPosition = transform.position;
        //chase the player if close enough
        if (Vector2.Distance(AIPosition, player.position) < chaseDist)
            AIMove(player);
        else
        {
            //check if we need to change goal
            WaypointUpdate();
            //move towards the goal
            AIMove(waypoints[waypointIndex].transform);
        }


    }

    private void AIMove(Transform goal)
    {
        Vector2 AIPosition = transform.position;
        {
            //if we're not near the goal
            if (Vector2.Distance(AIPosition, goal.position) > minGoalDist)
            {
                Vector2 dirToGoal = goal.position - transform.position;
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
    }

    private void WaypointUpdate()
    {
        Vector2 AIPosition = transform.position;
        //check if we need to change goal
        if (chasing)
        {
            int closePoint = 0;
            float dist = 0f;
            for (int i = 0; i < waypoints.Length; i++)
            {
                float tempDist = Vector2.Distance(AIPosition, waypoints[i].transform.position);
                //check if the distance to point i is smaller than point i-1
                //and check if that point is NOT the same as the previous point before chasing player
                if ((tempDist < dist || dist == 0) && (i != lastPoint))
                {
                    dist = tempDist;
                    closePoint = i;
                }
            }
            
            //then move to the nearest point
            waypointIndex = closePoint;
            chasing = false;
        }

        //once we reach the goal
        if (transform.position == waypoints[waypointIndex].transform.position) //(Vector2.Distance(AIPosition, waypoints[waypointIndex].transform.position) < minGoalDist)
        {
            lastPoint = waypointIndex;
            waypointIndex++;
            if (waypointIndex >= waypoints.Length)
            {
                waypointIndex = 0;
            }
        }
    }
}


/*          METHOD 1
if (transform.position.x < position0.transform.position.x)
{
    transform.position += Vector3.right * 1 * Time.deltaTime;
}
else
{
    transform.position -= Vector3.right * 1 * Time.deltaTime;
}

if (transform.position.y < position0.transform.position.y)
{
    transform.position += Vector3.up * 1 * Time.deltaTime;
}
else
{
    transform.position -= Vector3.up * 1 * Time.deltaTime;
}

            METHOD 2


AIPosition.x = AIPosition.x + (1 * Time.deltaTime);

transform.position = AIPosition;
*/

//transform.position = Vector2.MoveTowards(transform.position, position0.transform.position, 1 * Time.deltaTime);