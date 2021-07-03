using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackVelocityPotion : ItemController
{
    public InventoryManager inventoryManager;

    private void Start()
    {
        GameObject temp = GameObject.Find("Kevin");
        inventoryManager = temp.GetComponent<InventoryManager>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        // gameObject.SetActive(false);
        inventoryManager.addAttackVelocity();
        base.OnTriggerEnter(other);
    }
}
