using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    public float kunaiDamage = 25f;
    private void OnTriggerExit(Collider other)
    {
        /*Vemos si hemos dado a un enemigo*/
        if (other.tag == "Enemy")
        {
            if (other.name == "Spider_Flower")
            {
                BossStats bossStats = other.GetComponent<BossStats>();
                bossStats.TakeDamage(kunaiDamage);
            }
            else
            {
                // EnemyStats enemy = other.GetComponent<EnemyStats>();
                // enemy.TakeDamage(kunaiDamage);
                Destroy(gameObject);
            }
        }
    }
}
