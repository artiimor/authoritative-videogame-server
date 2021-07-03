using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : ItemContoller
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().addPocion();

            ServerSend.PotionAdded(other.gameObject.GetComponent<Player>().id);
            ServerSend.DestroyItem(id);
            Destroy(gameObject);
        }
    }
}
