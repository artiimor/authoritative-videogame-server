using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int id;

    private void FixedUpdate()
    {
        if (GameManager.items[id] != this)
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        // gameObject.SetActive(false);
        // inventoryManager.addPocion();
        // Destroy(gameObject);
    }
}
