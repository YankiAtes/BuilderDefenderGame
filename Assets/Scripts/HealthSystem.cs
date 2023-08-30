using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler onDamaged;
    public event EventHandler onHealed;
    public event EventHandler onDied;


    [SerializeField] private int healthAmountMax;
    private int healthAmount;

    private void Awake()
    {
        healthAmount = healthAmountMax;
    }

    public void Damage(int damageAmount)
    {
        healthAmount -= damageAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        onDamaged?.Invoke(this,EventArgs.Empty);

        if (IsDead())
        {
            onDied?.Invoke(this,EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        healthAmount += healAmount;
        Math.Clamp(healthAmount, 0 , healthAmountMax);
        onHealed?.Invoke(this,EventArgs.Empty);
    }

    public void HealFull()
    {
        healthAmount = healthAmountMax;
        onHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return healthAmount == 0;
    }

    public bool IsFullHealth()
    {
        return healthAmount == healthAmountMax;
    }

    public int GetHealthAmount()
    {
        return healthAmount;
    }

    public int GetHealthAmountMax()
    {
        return healthAmountMax;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int healthAmountMax, bool UpdateHealthAmount)
    {
        this.healthAmountMax = healthAmountMax; 
        if (UpdateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }
    }
}
