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
        _combatCanvas.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AIMovement aiMove = collision.collider.gameObject.GetComponent<AIMovement>();

        if (aiMove == null)
        {
            return;
        }

        enemy = collision.collider.gameObject;

        Debug.Log("This is an AI");

        _combatCanvas.SetActive(true);

        Time.timeScale = 0;

        if (aiManager.dead)
        aiManager.Refresh();
    }

    public void EndCombat()
    {
        Debug.Log("End Combat");
        _combatCanvas.SetActive(false);
        Time.timeScale = 1;
        Destroy(enemy);
    }
}
