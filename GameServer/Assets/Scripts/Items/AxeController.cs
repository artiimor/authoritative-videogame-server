using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : ItemContoller
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().addDamage();
            ServerSend.DestroyItem(id);
            Destroy(gameObject);
        }
    }
}
