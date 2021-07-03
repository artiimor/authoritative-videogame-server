using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkController : ItemController
{
    public InventoryManager inventoryManager;

    protected override void OnTriggerEnter(Collider other)
    {
        // gameObject.SetActive(false);
        // inventoryManager.addProteccion();
        base.OnTriggerEnter(other);
    }
}
