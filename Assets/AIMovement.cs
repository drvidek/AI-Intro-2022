using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public GameObject position0;
    public GameObject position1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 AIPosition = transform.position;

        /*
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
        */

        Vector2 dirToPos0 = position0.transform.position - transform.position;
        dirToPos0.Normalize();
        transform.position += (Vector3) dirToPos0 * 1 * Time.deltaTime;

        /*
        Vector2 AIPosition = transform.position;

        AIPosition.x = AIPosition.x + (1 * Time.deltaTime);

        transform.position = AIPosition;
        */

        //transform.position = Vector2.MoveTowards(transform.position, position0.transform.position, 1 * Time.deltaTime);
    }
}
