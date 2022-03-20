using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseManager : MonoBehaviour
{
    [Header("Health attributes")]
    [SerializeField] protected float _health = 100f;    //our current health
    [SerializeField] protected float _healthMax = 100f;     //our max possible health
    [SerializeField] protected Text _healthText;    //the text object to display our health
    [SerializeField] protected Text _healthMaxText; //the text object to display our max health

    protected virtual void Start()
    {
        UpdateHealthText();
    }

    public abstract void TakeTurn();

    public void Heal(float heal)
    {
        //add healing done to current health, with a highest possible value of our maximum health
        _health = Mathf.Min(_health + heal, _healthMax);
        UpdateHealthText();
    }

    public void DealDamage(float damage)
    {
        //subtract damage done from current health, with a lowest possible value of 0
        _health = Mathf.Max(_health - damage, 0);
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        //if you have a health text object, update it to match your current health
        if (_healthText != null)
            _healthText.text = _health.ToString();
        //if you have a max health text object, update it to match your max health
        if (_healthMaxText != null)
            _healthMaxText.text = _healthMax.ToString();
    }
}
