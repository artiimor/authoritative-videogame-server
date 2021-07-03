using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : EnemyStats
{
    private float damage = 20f;
    private float coolDown = 2f;
    private float specialCoolDown = 15f;

    private bool slowDown = true;

    bool hasUsedPoison = false;

    /*Extensiones*/
    public void TakeDamage(float damage)
    {
        currentHealth -= (float)(damage * 0.3);

        // hbcont.SetHealth(currentHealth);
    }

    public float getSpecialCoolDown()
    {
        return specialCoolDown; // Cooldown de la ralentizacion
    }

    public bool shouldUsePoison()
    {
        // Debug.Log(currentHealth + "");
        if (!hasUsedPoison)
        {
            hasUsedPoison = true;
            return (currentHealth) <= 25f;
        }
        return false;
    }

    public bool canSlowDown()
    {
        if (slowDown == true)
        {
            slowDown = false;
            Invoke(nameof(resetSlowDown), specialCoolDown);
            return true;
        }

        return false;
    }

    public void resetSlowDown()
    {
        slowDown = true;
    }
}
