using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointInvis : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        //get the sprite renderer for the waypoints and disable to make them invisible to the player
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
}
