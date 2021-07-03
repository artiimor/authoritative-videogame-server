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

    public LayerMask playerMask;

    void FixedUpdate()
    {
        if (!dead)
        {
            if (enemyStats.getCurrentHealth() > 0)
            {
                /*Comprobacion para la IA*/
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInDangerRange = Physics.CheckSphere(transform.position, dangerRange, whatIsPlayer);

                /*Obtenemos un jugador aleatorio en el rango del enemigo*/
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
                                ServerSend.EnemyTarget(this, auxCol.gameObject.GetComponent<Player>());
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
                    if (agent.speed == 0f)
                    {
                        agent.speed = 2.5f; // Quieto
                    }
                    if (player == null) Patroling();
                    else
                    {
                        float playerDistance = Vector3.Distance(player.position, transform.position);
                        // Debug.Log("playerDistance: " + playerDistance);

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
        /*Animaciones. Corre para alejarse*/
        // animator.SetBool("Patrolling", false);
        // animator.SetBool("Running", true);

        // playReeSound();

        // Debug.Log("huyendo y tal");

        if (player != null)
        {
            Vector3 dirToPlayer = transform.position - player.transform.position;
            transform.rotation = Quaternion.LookRotation(dirToPlayer);

            /*La nueva direccion va en contra de la del jugador*/
            Vector3 newPos = transform.position + dirToPlayer;

            agent.SetDestination(newPos);
            ServerSend.UpdateEnemyPosition(this, newPos);
        }
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/

    private void AttackYoQSETIO()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50f, playerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            Player playerAux = hit.transform.gameObject.GetComponent<Player>();
            playerAux.getDamage(25f);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        canRotate = true;
    }
    protected override void AttackPlayer()
    {
        base.AttackPlayer();

        // Debug.Log("Attack y tal");

        /*Se está quieto porque tiene que apuntar*/
        // animator.SetBool("Patrolling", false);
        // animator.SetBool("Running", false);

        /*Comprobación del cooldown*/
        

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;

            // transform.rotation = _rotation;
            canRotate = false;
            ServerSend.DistanceEnemyAttack(this);

            float Distance = Vector3.Distance(transform.position, player.position);
            float TimeBetweenObjects = Distance / 28f;

            Invoke(nameof(AttackYoQSETIO), TimeBetweenObjects);
            Invoke(nameof(ResetAttack), coolDown);

            

            // Ataca
            // animator.SetTrigger("Attack");

            // Disparamos la flecha
            /* GameObject arrow = Instantiate(projectile, transform.position + new Vector3(0f, 0.5f, 0f), Quaternion.identity);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            arrow.transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y + 90f, 0f);
            arrow.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (transform.forward * 0.5f);

            rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);

            // ara evitar que tengamos infinitas flechas en el juego de manera simultanea
            Destroy(arrow, 3f);

            ServerSend.DistanceEnemyAttack(this);
            
            // Reseteamos el coolDown
            Invoke(nameof(ResetAttack), coolDown);*/
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /*********************************************************************************/
    /*********************************FUNCIONES EXTRA*********************************/
    /*********************************************************************************/
    
    public override void Die()
    {
        base.Die();

        // Debug.Log("Matándolo:");
        /*Dropea compadre*/
        int n = Random.Range(1, 101);

        if (n < 30)
        {
            GameObject go = Instantiate(item, transform.position, transform.rotation);
            ItemContoller ic = go.GetComponent<ItemContoller>();
            ServerSend.SpawnMilk(ic.id, go.transform.position);
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
