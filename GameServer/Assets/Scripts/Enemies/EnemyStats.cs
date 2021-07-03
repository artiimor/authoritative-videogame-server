using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    protected float currentHealth = 100f;
    private float damage = 15f;
    private float coolDown = 5f;
    
    // public HealthbarController hbcont;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        Debug.Log("Haciendole damages");

        ServerSend.EnemyHurt(this.gameObject.GetComponent<Enemy>().id, getCurrentHealth());

        if (currentHealth <= 0f)
        {
            this.GetComponent<Enemy>().Die();
        }
    }

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
}
