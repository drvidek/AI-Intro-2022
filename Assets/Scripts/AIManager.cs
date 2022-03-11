using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : BaseManager
{


    public enum State
    {
        fullHP,
        lowHP,
        dead
    }
    public State currentState;

    protected PlayerManager _player;


    protected override void Start()
    {
        base.Start();
        _player = GetComponent<PlayerManager>();
        if (_player == null)
        {
            Debug.Log("Player Manager Not Found");
        }
    }

    public override void TakeTurn()
    {
        if (_health < 40)
            currentState = State.lowHP;
                else
            currentState = State.fullHP;

        if (_health == 0)
            currentState = State.dead;

        switch (currentState)
        {
            case State.fullHP:
                FullHPState();
                break;
            case State.lowHP:
                LowHPState();
                break;
            case State.dead:
                DeadState();
                break;
            default:
                break;
        }

        if (_health > 0)
        StartCoroutine(EndTurn());
    }


    IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2f);
        _player.TakeTurn();
        Debug.Log("Player Turn");
    }

    public void FullHPState()
    {
        int randomAttack = Random.Range(0, 10);
        if (randomAttack <= 2)
        {
            RazorLeaf();
        }
        else
            if (randomAttack <= 8)
        {
            VineWhip();
        }
        else
        {
            SelfImmolate();
        }
    }

    public void LowHPState()
    {
        int randomAttack = Random.Range(0, 10);
        if (randomAttack <= 3)
        {
            Synthesis();
        }
        else
            if (randomAttack <= 6)
        {

            RazorLeaf();
        }
        else
            if (randomAttack <= 7)
        {
            VineWhip();
        }
        else
        {
            SelfImmolate();
        }
    }

    public void DeadState()
    {
        Debug.Log("You Win!");
    }

    public void VineWhip()
    {
        _player.DealDamage(15f);
        Debug.Log("VineWhip");
    }

    public void RazorLeaf()
    {
        _player.DealDamage(25f);
        Debug.Log("RazorLeaf");
    }


    public void Synthesis()
    {
        Heal(25f);
        Debug.Log("Heal");
    }

    public void SelfImmolate()
    {
        DealDamage(_maxHealth);
        _player.DealDamage(90f);
        Debug.Log("SD");
    }
}
