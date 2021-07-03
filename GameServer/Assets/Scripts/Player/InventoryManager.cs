using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InventoryManager : MonoBehaviour
{
    public int n_pociones = 0;
    private int n_proteccion = 0;
    private int n_damage = 0;
    public float potionRestoration = 20f;

    /*Singleton*/
    public static InventoryManager inventoryManager;

    void Start()
    {
        /*Instanciacion del singleton*/
        /*if (inventoryManager == null)
        {
            inventoryManager = this;
        }
        else
        {
            Destroy(this);
        }*/
    }

    public void addPocion()
    {
        n_pociones++;
    }

    public void addProteccion()
    {
        /*Maximo podemos sumar 10 protecciones (mitad del daño)*/
        if (n_proteccion < 10)
        {
            n_proteccion++;
        }
    }

    public void addDamage()
    {
        /*Maximo podemos sumar 10 protecciones (mitad del daño)*/
        if (n_damage < 10)
        {
            n_damage++;
        }
    }

    public void delPocion()
    {
        n_pociones--;
    }

    public bool pocionesDisponibles()
    {
        return n_pociones > 0;
    }

    public float getPotionRestoration()
    {
        return potionRestoration;
    }

    public double getProteccionMod()
    {
        return 1 - (n_proteccion * 0.1);
    }

    public double getDamageMod()
    {
        /*Solo afecta al ataque a melee*/
        return (1 + n_damage * 0.1);
    }

    public void addMovementPocion()
    {
        NavMeshAgent agent = GetComponentInParent<NavMeshAgent>();
        agent.speed = agent.speed * 1.5f;
    }

    public void addAttackVelocity()
    {
        PlayerCooldown pc = GetComponentInParent<PlayerCooldown>();
        Debug.Log(pc.cooldown + "");
        pc.cooldown = pc.cooldown * 0.5f;
        Debug.Log("Cooldown disminuido :))");
        Debug.Log(pc.cooldown + "");
    }

}
