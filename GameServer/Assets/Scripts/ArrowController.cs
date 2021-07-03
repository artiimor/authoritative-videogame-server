using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float arrowDamage = 25f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("other.tag: " + other.gameObject.name);
        /*Vemos si hemos dado a un enemigo*/
        if (other.tag == "Player")
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            player.getDamage(arrowDamage);
            Destroy(gameObject);
        }
    }
}
