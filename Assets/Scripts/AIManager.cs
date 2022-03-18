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

    public bool dead;

    [SerializeField] protected StartCombat startCombat;

    protected PlayerManager _player;
    [SerializeField] protected Animator _anim;


    protected override void Start()
    {
        base.Start();
        _player = GetComponent<PlayerManager>();
        if (_player == null)
        {
            Debug.Log("Player Manager Not Found");
        }

        
    }

    public void Refresh()
    {
        if (_health < _healthMax)
            _health = _healthMax;
        currentState = State.fullHP;
        dead = false;
        UpdateHealthText();
        _player.TakeTurn();
    }

    public override void TakeTurn()
    {
        if (_health < _healthMax / 2)
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
        yield return new WaitForSecondsRealtime(2f);
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
        
        StartCoroutine(EndCombat());
        
    }

    IEnumerator EndCombat()
    {
        if (!dead)
        {
            dead = true;
            yield return new WaitForSecondsRealtime(1f);
            startCombat.EndCombat();
        }

    }


    #region Attacks
    public void VineWhip()
    {
        _anim.SetBool("isEnemy", true);
        _player.DealDamage(15f);
        Debug.Log("VineWhip");
        _anim.SetTrigger("VineWhip");
    }

    public void RazorLeaf()
    {
        _player.DealDamage(25f);
        Debug.Log("RazorLeaf");
        _anim.SetTrigger("RazorLeaf");
    }


    public void Synthesis()
    {
        Heal(25f);
        Debug.Log("Heal");
        _anim.SetTrigger("Synthesis");
    }

    public void SelfImmolate()
    {
        DealDamage(_healthMax);
        _player.DealDamage(90f);
        _anim.SetTrigger("Immolate");
        Debug.Log("SD");
    }
    #endregion


}
