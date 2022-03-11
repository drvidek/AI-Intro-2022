using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : BaseManager
{

    private AIManager _aiManager;
    [SerializeField] protected CanvasGroup _playerButtons;

    protected override void Start()
    {
        base.Start();
        _aiManager = GetComponent<AIManager>();
        if (_aiManager == null)
        {
            Debug.Log("AI Manager Not Found");
        }
    }

    public override void TakeTurn()
    {
        _playerButtons.interactable = true;
    }


    public void VineWhip()
    {
        _aiManager.DealDamage(15f);
        StartCoroutine( EndTurn());
    }

    public void RazorLeaf()
    {
        _aiManager.DealDamage(25f);
        StartCoroutine(EndTurn());
    }


    public void Synthesis()
    {
        Heal(25f);
        StartCoroutine(EndTurn());
    }

    public void SelfImmolate()
    {
        DealDamage(_maxHealth);
        _aiManager.DealDamage(90f);
        StartCoroutine(EndTurn());
    }


    IEnumerator EndTurn()
    {
        Debug.Log("AI Turn");
        _playerButtons.interactable = false;
        yield return new WaitForSeconds(2f);
        _aiManager.TakeTurn();
        
    }
}
