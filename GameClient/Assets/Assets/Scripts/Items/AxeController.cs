using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : ItemController
{
    public InventoryManager inventoryManager;

    protected override void OnTriggerEnter(Collider other)
    {
        // gameObject.SetActive(false);
        // inventoryManager.addDamage();
        base.OnTriggerEnter(other);
    }
}
