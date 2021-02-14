using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealthSystem
{
    int health;//current health

    public HealthSystem(int health)//set health
    {
        this.health = health;
    }
    public int GetHealth()//get health
    {
        return this.health;
    }
    /// <summary>
    /// cause damage to character
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount)
    {
        this.health -= amount;
        if (this.health < 0)
        {
            this.health = 0;
        }
    }
    /// <summary>
    /// funtion to heal character
    /// </summary>
    /// <param name="amountHeal"></param>
    public void Heal(int amountHeal)
    {
        this.health += amountHeal;
        if (this.health > GameManager.Instance.maxHealth)
        {
            this.health = GameManager.Instance.maxHealth;
        }
    }
    /// <summary>
    /// function to check death of character
    /// </summary>
    /// <returns></returns>
    public bool IsDead()
    {
        return this.health <= 0;
    }

}
