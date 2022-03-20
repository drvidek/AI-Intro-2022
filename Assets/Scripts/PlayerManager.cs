using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager
{

    private AIManager _aiManager;   //to store our AIManager component
    [SerializeField] protected CanvasGroup _playerButtons;  //to store our combat buttons

    protected override void Start()
    {
        //inherit start from BaseManager
        base.Start();
        //store the AIManager component
        _aiManager = GetComponent<AIManager>();
        if (_aiManager == null)
        {
            Debug.Log("AI Manager Not Found");
        }
    }

    public override void TakeTurn()
    {
        //activate the player combat buttons
        _playerButtons.interactable = true;
    }

    #region Combat Moves

    public void VineWhip()
    {
        //deal 15 damage to the enemy and end our turn
        _aiManager.DealDamage(15f);
        StartCoroutine(EndTurn());
    }

    public void RazorLeaf()
    {
        //deal 25 damage to the enemy and end our turn
        _aiManager.DealDamage(25f);
        StartCoroutine(EndTurn());
    }


    public void Synthesis()
    {
        //heal ourselves for 25 health and end our turn
        Heal(25f);
        StartCoroutine(EndTurn());
    }

    public void SelfImmolate()
    {
        //deal 90 damage to the enemy, reduce our health to 1, and end our turn
        DealDamage(_health - 1);
        _aiManager.DealDamage(90f);
        StartCoroutine(EndTurn());
    }
    #endregion

    IEnumerator EndTurn()
    {
        Debug.Log("AI Turn");
        //disable the player combat buttons
        _playerButtons.interactable = false;
        //wait 1.5 seconds
        yield return new WaitForSecondsRealtime(1.5f);
        //tell the AI to take their turn
        _aiManager.TakeTurn();

    }
}
