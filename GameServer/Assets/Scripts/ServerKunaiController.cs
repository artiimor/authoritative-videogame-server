using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerKunaiController : MonoBehaviour
{
    public float kunaiDamage = 25f;
    private void OnTriggerExit(Collider other)
    {
        /*Vemos si hemos dado a un enemigo*/
        if (other.tag == "Player")
        {
            // EnemyStats enemy = other.GetComponent<EnemyStats>();
            // enemy.TakeDamage(kunaiDamage);
            // Destroy(gameObject);
            Debug.Log("hemos alcanzado a un jugador");
        }
    }
}
