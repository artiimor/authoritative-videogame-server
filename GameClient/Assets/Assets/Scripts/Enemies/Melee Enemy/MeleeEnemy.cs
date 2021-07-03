using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.enemies[id] != this)
        {
            Destroy(this.gameObject);
        }

        if (!dead)
        {
            if (enemyStats.getCurrentHealth() > 0)
            {
                /*Comprobacion para la IA*/
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

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
                    if (player == null) Patroling(); // Patrolling si no tiene jugador target. Solo modifica las animaciones
                    else
                    {
                        float playerDistance = Vector3.Distance(player.position, transform.position);

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
        animator.SetBool("Patrolling", false);
        animator.SetBool("Running", true);

        // audioSource.PlayOneShot(reee);

        /*Gotta go fast*/
        agent.speed = 4f;

        /*Quiere ir a donde está el jugador*/
        agent.SetDestination(player.position);
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/
    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        /*Deja de correr, puto anormal*/
        animator.SetBool("Running", false);
    }

    public override void AttackReceived()
    {
        // Debug.Log("Recibido paquete de enemigo atacando LET'S GOO");
        /*Ataca*/
        animator.SetTrigger("Attack");
        player.gameObject.GetComponent<PlayerCombat>().getDamage(enemyStats.getDamage());
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected override void Die()
    {
        base.Die();

        /*Avisamos al scoreManager*/
        // GameObject scripter = GameObject.Find("scripter");
        // scripter.GetComponent<ScoreManager>().meleeKilled();
    }
}

