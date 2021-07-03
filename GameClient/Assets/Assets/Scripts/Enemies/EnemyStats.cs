using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    protected float currentHealth = 100f;
    private float damage = 15f;
    private float coolDown = 4f;
    
    public HealthbarController hbcont;

    /* public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        hbcont.SetHealth(currentHealth);
    }*/

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public float getDamage()
    {
        return damage;
    }

    public float getCoolDown()
    {
        return coolDown;
    }

    public void setHealth(float health)
    {
        currentHealth = health;

        // hbcont.SetHealth(currentHealth);
    }

    public float getHealth()
    {
        return currentHealth;
    }
}
