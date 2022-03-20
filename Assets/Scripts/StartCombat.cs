using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCombat : MonoBehaviour
{
    [SerializeField] GameObject _combatCanvas;
    public AIManager aiManager;
    public GameObject enemy;

    private void Start()
    {
        //make sure the canvas is off when the game starts
        _combatCanvas.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)      ///when you collide with something
    {
        //try to get an AIMovement component off the object (i.e. this will be an AI agent)
        AIMovement aiMove = collision.collider.gameObject.GetComponent<AIMovement>();

        //if we didn't find an AIMovement component, exit the method
        if (aiMove == null)
        {
            return;
        }

        //get the enemy instance you collided with, so we can delete it later
        enemy = collision.collider.gameObject;

        Debug.Log("This is an AI");

        //activate the combat canvas
        _combatCanvas.SetActive(true);

        //pause time so the AI agents don't move in the background
        Time.timeScale = 0;

        //if the AI manager combat script is still dead from previous combat, refresh it for new combat
        if (aiManager.dead)
        {
            aiManager.Refresh();
        }
    }

    public void EndCombat()     //for when the enemy has been defeated
    {
        Debug.Log("End Combat");

        //disable the combat screen
        _combatCanvas.SetActive(false);

        //start time up again
        Time.timeScale = 1;

        //destroy the enemy instance you defeated
        Destroy(enemy);
    }
}
