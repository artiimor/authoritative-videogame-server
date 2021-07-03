using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float arrowDamage = 25f;
    private void OnTriggerExit(Collider other)
    {
        /*Vemos si hemos dado a un enemigo*/
        if (other.tag == "Player")
        {
            // PlayerCombat player = other.GetComponent<PlayerCombat>();
            // player.getDamage(arrowDamage);
            Destroy(gameObject);
        }        
    }
}
