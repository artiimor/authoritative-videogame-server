using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DistanceEnemy : Enemy
{
    /*Para el estado de ataque*/
    public float coolDown;
    public GameObject projectile;

    /*Para los estados de la IA*/
    public bool playerInDangerRange;
    public float dangerRange;

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
                playerInDangerRange = Physics.CheckSphere(transform.position, dangerRange, whatIsPlayer);

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
                    if (agent.speed == 0f)
                    {
                        agent.speed = 2.5f; // Quieto
                    }
                    if (player == null) Patroling();
                    else
                    {
                        float playerDistance = Vector3.Distance(player.position, transform.position);

                        if (playerDistance <= sightRange && playerDistance > dangerRange) AttackPlayer();
                        if (playerDistance <= sightRange && playerDistance <= dangerRange) GetDistance();
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

    /**********************************************************************************/
    /****************************ESTADO DE HUIR DEL JUGADOR****************************/
    /**********************************************************************************/
    private void GetDistance()
    {
        if (agent.destination != transform.position)
        {
            /*Animaciones. Corre para alejarse*/
            animator.SetBool("Patrolling", false);
            animator.SetBool("Running", true);

            playReeSound();

            Vector3 dirToPlayer = transform.position - player.transform.position;


            transform.rotation = Quaternion.LookRotation(dirToPlayer);
        }

        /* 
        transform.rotation = Quaternion.LookRotation(dirToPlayer);

        // La nueva direccion va en contra de la del jugador
        Vector3 newPos = transform.position + dirToPlayer;

        agent.SetDestination(newPos);*/
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/
    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        /*Se está quieto porque tiene que apuntar*/
        animator.SetBool("Patrolling", false);
        animator.SetBool("Running", false);

        /*Comprobación del cooldown*/
        /* if (!alreadyAttacked)
        {

            alreadyAttacked = true;

            // Ataca
            animator.SetTrigger("Attack");

            // Disparamos la flecha
            GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            arrow.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 90f, 0f);
            arrow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (transform.forward * 0.5f);

            rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);

            // Para evitar que tengamos infinitas flechas en el juego de manera simultanea
            Destroy(arrow, 3f);

            // Reseteamos el coolDown
            Invoke(nameof(ResetAttack), coolDown);
        }*/
    }

    public override void AttackReceived()
    {
        // Ataca
        animator.SetTrigger("Attack");

        // Disparamos la flecha
        GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        arrow.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 90f, 0f);
        arrow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (transform.forward * 0.5f);

        rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
        rb.AddForce(transform.up * 5f, ForceMode.Impulse);

        // Para evitar que tengamos infinitas flechas en el juego de manera simultanea
        Destroy(arrow, 3f);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /*********************************************************************************/
    /*********************************FUNCIONES EXTRA*********************************/
    /*********************************************************************************/
    protected override void Die()
    {
        base.Die();

        /*Avisamos al scoreManager*/
        // GameObject scripter = GameObject.Find("scripter");
        // scripter.GetComponent<ScoreManager>().distanceKilled();
    }

    
}
