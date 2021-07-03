using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dead)
        {
            if (enemyStats.getCurrentHealth() > 0)
            {
                /*Comprobacion para la IA*/
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                /*Obtenemos un jugador aleatorio en el rango del enemigo*/
                /*Lo obtenemos si está sin jugador target solo*/
                if (player == null) 
                {
                    if (playerInSightRange)
                    {
                        Collider[] aux = Physics.OverlapSphere(transform.position, sightRange);
                        foreach (Collider auxCol in aux)
                        {
                            if (auxCol.gameObject.layer == LayerMask.NameToLayer("player"))
                            {
                                player = auxCol.gameObject.transform;
                                playerComponent = auxCol.gameObject.GetComponent<Player>();

                                // Inform clients
                                ServerSend.EnemyTarget(this, playerComponent);
                                break;
                            }
                        }
                    }
                }
                if (player != null)
                {
                    if (!playerInSightRange)
                    {
                        player = null;
                        playerComponent = null;

                        // Inform clients
                        ServerSend.RemoveEnemyTarget(this);
                    }
                }

                /*Comprobacion para el knockback*/
                if (knockBackCheck)
                {
                    // Copiado de player movement
                    Quaternion newRotation = Quaternion.LookRotation(player.position - transform.position, Vector3.forward);

                    // Lo empujamos
                    rb.AddForce(-(newRotation * Vector3.forward * 15), ForceMode.Impulse);

                    knockBackCheck = false;
                }

                if (!stunned)
                {
                    /*Cambiamos el estado de la IA*/
                    if (player == null) Patroling();
                    else
                    {
                        float playerDistance = Vector3.Distance(player.position, transform.position);
                        // Debug.Log("playerDistance: " + playerDistance);

                        if (playerDistance <= sightRange && playerDistance > attackRange) ChasePlayer();
                        if (playerDistance <= sightRange && playerDistance <= attackRange) AttackPlayer();
                    }
                }
                else
                {
                    Stunned();
                }
            }

                
            else
            {
                Die();
            }
        }
    }

    /*********************************************************************************/
    /**************************ESTADO DE BUSQUEDA AL JUGADOR**************************/
    /*********************************************************************************/
    protected override void Patroling()
    {
        /*Velocidad de andar*/
        agent.speed = 2.5f;

        base.Patroling();
    }

    /**********************************************************************************/
    /**************************ESTADO DE PERSEGUIR AL JUGADOR**************************/
    /**********************************************************************************/
    private void ChasePlayer()
    {
        /*Animaciones. Corre para pegarnos una paliza a navajazos*/
        // animator.SetBool("Patrolling", false);
        // animator.SetBool("Running", true);

        // audioSource.PlayOneShot(reee);

        /*Gotta go fast*/
        agent.speed = 4f;

        /*Quiere ir a donde está el jugador*/
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/
    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        /*Deja de correr, puto anormal*/
        // animator.SetBool("Running", false);

        /*Comprobación del cooldown*/
        if (!alreadyAttacked)
        {
            /*Ataca*/
            // animator.SetTrigger("Attack");
            ServerSend.MeleeEnemyAttack(this);

            /*No es "preciso", pero es lo que mejor queda jugablemente*/
            playerComponent.getDamage(enemyStats.getDamage());


            /*Reseteamos el coolDown*/
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), enemyStats.getCoolDown());
        }
    }
    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public override void Die()
    {
        base.Die();

        Debug.Log("Matándolo:");
        /*Dropea compadre*/
        int n = Random.Range(1, 101);
        if (n < 30)
        {
            GameObject go = Instantiate(item, transform.position, transform.rotation);
            ItemContoller ic = go.GetComponent<ItemContoller>();
            ServerSend.SpawnAxe(ic.id, go.transform.position);
        }
        if (n > 59)
        {
            GameObject go = Instantiate(potion, transform.position, transform.rotation);
            ItemContoller ic = go.GetComponent<ItemContoller>();
            ServerSend.SpawnPotion(ic.id, go.transform.position);
        }

        remove();
    }
}

