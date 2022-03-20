using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIManager : BaseManager
{

    [Header("State details")]
    public State currentState;  //store our current state
    public enum State   //enumerator for our states
    {
        fullHP,
        lowHP,
        dead
    }

    [Header("Combat details")]
    public bool dead;   //trigger for end of combat
    [SerializeField] protected StartCombat startCombat; //to store our start combat script
    protected PlayerManager _player;    //to store our player manager script
    [SerializeField] protected Animator _anim;  //to store our animator
    [SerializeField] protected Text _attacktext;    //to store our attack display text
    [SerializeField] protected Image _image;    //to store our image


    protected override void Start()
    {
        //inherit start from BaseManager
        base.Start();
        //get the player manager script component
        _player = GetComponent<PlayerManager>();
        //Set the attack text to be blank
        _attacktext.text = "";
    }

    public void Refresh()   //for the start of a new round of combat
    {
        //if my health is below maximum, set it back to max
        if (_health < _healthMax)
            _health = _healthMax;

        //set my state to fullHP
        currentState = State.fullHP;

        //set my dead trigger to false
        dead = false;

        //set attack text to blank
        _attacktext.text = "";

        //refresh the health display
        UpdateHealthText();

        //start combat with the player's turn
        _player.TakeTurn();
    }

    public override void TakeTurn()
    {
        //set my state based on my remaining health
        if (_health <= _healthMax / 2)
            currentState = State.lowHP;
        else
            currentState = State.fullHP;

        if (_health == 0)
            currentState = State.dead;

        switch (currentState)   //trigger the appropriate state actuib
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

        //if i'm still alive, end my turn
        if (_health > 0)
            StartCoroutine(EndTurn());
    }


    IEnumerator EndTurn()
    {
        //wait for 2 seconds
        yield return new WaitForSecondsRealtime(2f);
        //start the player turn
        _player.TakeTurn();
        //set attack text to blank
        _attacktext.text = "";
    }

    public void FullHPState()
    {
        int randomAttack = Random.Range(0, 10); //pick a random number from 0 to 9 inclusive

        //30% chance
        if (randomAttack <= 2)
        {
            Thrash();
        }
        else
        //70% chance
        {
            Tackle();
        }
    }

    public void LowHPState()
    {
        int randomAttack = Random.Range(0, 10);

        //30% chance
        if (randomAttack <= 2)
        {
            EatBerry();
        }
        else
        //50% chance
            if (randomAttack <= 7)
        {

            Thrash();
        }
        else
        //20% chance
        {
            Tackle();
        }
    }

    public void DeadState()
    {
        //set the attack text
        _attacktext.text = "Defeated!";
        //statt the end of combat
        StartCoroutine(EndCombat());

    }

    IEnumerator EndCombat()
    {
        //if this is the first frame of being dead
        if (!dead)
        {
            //set our dead check to true
            dead = true;
            //activate our animation
            _anim.SetBool("Dead", true);
            //wait 1 second for it to play
            yield return new WaitForSecondsRealtime(1f);
            //reset our animation
            _anim.SetBool("Dead", false);
            yield return null;
            //finalise the end combat
            startCombat.EndCombat();
        }

    }


    #region Combat Moves
    public void Tackle()
    {
        //deal 10 damage
        _player.DealDamage(10f);
        //set the attack text
        _attacktext.text = "Tackle";
        //tell the animator we should access the enemy version of the animation
        _anim.SetBool("isEnemy", true);
        //use the vine whip animation
        _anim.SetTrigger("VineWhip");
    }

    public void Thrash()
    {
        _player.DealDamage(20f);
        _attacktext.text = "Thrash";
        _anim.SetTrigger("RazorLeaf");
    }


    public void EatBerry()
    {
        Heal(10f);
        _attacktext.text = "Eat Berry";
        _anim.SetTrigger("Synthesis");
    }
    #endregion


}
