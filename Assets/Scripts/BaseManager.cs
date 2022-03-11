using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseManager : MonoBehaviour
{
    [SerializeField] protected float _health = 100f;
    [SerializeField] protected float _maxHealth = 100f;

    [SerializeField] protected Text _healthText;

    protected virtual void Start()
    {
        UpdateHealthText();

    }

    public abstract void TakeTurn();

    public void Heal(float heal)
    {
        _health = Mathf.Min(_health + heal, _maxHealth);
        UpdateHealthText();
    }

    public void DealDamage(float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        if (_health == 0)
        {
            Debug.Log("I died");
        }
        UpdateHealthText();
    }

    public  void    UpdateHealthText()
    {
        if (_healthText != null)
        _healthText.text = _health.ToString();
    }
}
