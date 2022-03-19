using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] protected Text _attacktext;
    [SerializeField] protected Image _image;


    protected override void Start()
    {
        base.Start();
        _player = GetComponent<PlayerManager>();
        if (_player == null)
        {
            Debug.Log("Player Manager Not Found");
        }
        _attacktext.text = "";
    }

    public void Refresh()
    {
        if (_health < _healthMax)
            _health = _healthMax;
        currentState = State.fullHP;
        dead = false;
        _attacktext.text = "";
        _image.enabled = true;
        UpdateHealthText();
        _player.TakeTurn();
    }

    public override void TakeTurn()
    {
        if (_health <= _healthMax / 2)
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
        _attacktext.text = "";
    }

    public void FullHPState()
    {
        int randomAttack = Random.Range(0, 10);
        if (randomAttack <= 3)
        {
            RazorLeaf();
        }
        else
        {
            VineWhip();
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
            if (randomAttack <= 7)
        {

            RazorLeaf();
        }
        else
        {
            VineWhip();
        }
    }

    public void DeadState()
    {
        _attacktext.text = "Defeated!";
        
        StartCoroutine(EndCombat());

    }

    IEnumerator EndCombat()
    {
        if (!dead)
        {
            dead = true;
            _anim.SetBool("Dead", true);
            yield return new WaitForSecondsRealtime(1f);
            _anim.SetBool("Dead", false);
            yield return null;
            startCombat.EndCombat();
        }

    }


    #region Attacks
    public void VineWhip()
    {
        _player.DealDamage(15f);
        _attacktext.text = "Tackle";
        _anim.SetBool("isEnemy", true);
        _anim.SetTrigger("VineWhip");
    }

    public void RazorLeaf()
    {
        _player.DealDamage(25f);
        _attacktext.text = "Thrash";
        _anim.SetTrigger("RazorLeaf");
    }


    public void Synthesis()
    {
        Heal(10f);
        _attacktext.text = "Eat Berry";
        _anim.SetTrigger("Synthesis");
    }
    #endregion


}
