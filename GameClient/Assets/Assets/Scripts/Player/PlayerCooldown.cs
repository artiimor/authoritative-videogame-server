using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldown : MonoBehaviour
{
    private float lastAttackTime = 0;
    public float cooldown = 0.75f;
    public bool canAttack()
    {
        return (Time.time > lastAttackTime);
    }

    public void updateLastAttack()
    {
        lastAttackTime = Time.time + + cooldown;
    }
}
