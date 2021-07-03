using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : ItemContoller
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().addProteccion();
            ServerSend.DestroyItem(id);
            Destroy(gameObject);
        }
    }
}