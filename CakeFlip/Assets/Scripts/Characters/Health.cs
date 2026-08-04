using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 100;
    [SerializeField] private int maxHealth = 100;

    public event Action OnDeath;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health < 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
