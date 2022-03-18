using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    private SpriteRenderer spriteRenderer;
    private Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        //establish a Vector2 for our move direction
        Vector2 moveDir = Vector2.zero;

        //take player input and set direction accordingly
        if (Input.GetKey(KeyCode.W))
        {
            moveDir.y += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDir.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir.x += speed * Time.deltaTime;
            //make the sprite face right
            spriteRenderer.flipX = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir.x -= speed * Time.deltaTime;
            //make the sprite face left
            spriteRenderer.flipX = true;

        }

        //set the move animation in the animator controller
        if (moveDir != Vector2.zero)
        {
            _anim.SetBool("Moving", true);
        }
        else
        {
            _anim.SetBool("Moving", false);
        }

        //move the player object
        transform.position += (Vector3)moveDir;
    }
}
