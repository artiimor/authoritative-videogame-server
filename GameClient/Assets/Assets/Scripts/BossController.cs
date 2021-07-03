using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{

    /*Animaciones*/
    public Animator animator;

    /*Para relacionarnos con el jugador*/
    public Transform player;
    public PlayerCombat playerCombat;

    /*Para el movimiento*/
    public LayerMask whatIsGround, whatIsPlayer;
    public UnityEngine.AI.NavMeshAgent agent;

    public bool dead = false;

    public BossStats bossStats;

    /*Para el estado de busqueda*/
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    /*Para el estado de ataque*/
    bool alreadyAttacked;

    /*Parte principal de los estados de la IA*/
    public float attackRange;
    public bool playerInAttackRange;

    /*Para el knockback*/
    Rigidbody rb;
    bool knockBackCheck = false;
    bool stunned = false;
    public float stunTime = 2f; // Tiempo de duracion del stun
    float timer;

    /*Efecto de veneno*/
    bool hasAppliedPoison = false;

    /*Sonido*/
    protected AudioSource audioSource;
    public AudioClip spawn;
    public AudioClip reee;
    public AudioClip attacking;
    public AudioClip die;
    public AudioClip walk;

    // Start is called before the first frame update
    void Start()
    {

        // Jugador
        GameObject temp = GameObject.Find("Kevin");
        playerCombat = temp.GetComponent<PlayerCombat>();
        Transform kevinTransform = temp.GetComponent<Transform>();
        playerCombat = temp.GetComponent<PlayerCombat>();

        player = kevinTransform;

        // movimiento
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Fisicas
        rb = this.GetComponent<Rigidbody>();

        // Estados
        bossStats = this.GetComponent<BossStats>();
        // bossStats.TakeDamage(90f/0.3f);

        // Audio
        audioSource = GetComponent<AudioSource>();
        playSpawnSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            if (bossStats.getCurrentHealth() > 0)
            {
                /*Comprobacion para la IA*/
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
                    if (agent.speed == 0f)
                    {
                        agent.speed = 6f; // Quieto
                    }
                    /*Cambiamos el estado de la IA*/
                    if (!playerInAttackRange) ChasePlayer();
                    if (playerInAttackRange) AttackPlayer();
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
    /**************************ESTADO DE PERSEGUIR AL JUGADOR**************************/
    /**********************************************************************************/
    private void ChasePlayer()
    {
        /*Animaciones. Corre para pegarnos una paliza a navajazos*/
        animator.SetBool("Walking", true);

        /*Gotta go fast*/
        // agent.speed = 4f;

        /*Quiere ir a donde está el jugador*/
        agent.SetDestination(player.position);
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/
    private void AttackPlayer()
    {
        /*Deja de correr, puto anormal*/
        animator.SetBool("Walking", false);

        /*Se para y mira hacia el jugador*/
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        /*Comprobaciones para debuffs del boss*/
        /*Veneno*/
        if (!hasAppliedPoison && bossStats.shouldUsePoison())
        {
            // Debug.Log("Envenenando");
            playerCombat.getPoisoned();
        }

        /*Ralentizacion*/
        // // Debug.Log(bossStats.canSlowDown());
        if (bossStats.canSlowDown())
        {
            playerCombat.getSlowDown();
        }

        /*Comprobación del cooldown*/
        if (!alreadyAttacked)
        {
            /*Ataca*/
            animator.SetTrigger("Attack");

            /*bajamos vida al jugador*/
            playerCombat.getDamage(bossStats.getDamage()); // Pega el doble que los enemigos

            /*Reset del cooldown*/
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), bossStats.getCoolDown());
        }
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void Stunned()
    {
        /*Animaciones. Nos ponemos en idle*/
        animator.SetBool("Walking", false);

        timer += Time.deltaTime;

        /*Quietito*/
        walkPoint = transform.position;

        /*Comprobamos si ha pasado el tiempo del stun*/
        if (timer >= stunTime)
        {
            stunned = false;
        }
    }

    public void knockBack()
    {
        knockBackCheck = true;

        // transform.position += Vector3.back * 50;
        // rb.AddForce(new Vector3(10, 0, 0), ForceMode.Impulse);
    }

    public void getStun()
    {
        timer = 0f; // Reseteamos el timer
        agent.speed = 0f; // Quieto
        stunned = true; // Modo stun
    }

    private void Die()
    {
        /*Quieto*/
        agent.velocity = Vector3.zero;

        /*Animacion para que muera*/
        animator.SetTrigger("Dead");

        GetComponent<Collider>().enabled = false;
        agent.radius = 0.01f;
        agent.velocity = Vector3.zero;
        agent.SetDestination(transform.position);
        dead = true;

        playDeathSound();
    }

    private void playSpawnSound()
    {
        audioSource.PlayOneShot(spawn);
    }

    private void playAttackSound()
    {
        audioSource.PlayOneShot(attacking);
    }

    private void playWalkSound()
    {
        audioSource.PlayOneShot(walk);
    }

    private void playDeathSound()
    {
        audioSource.PlayOneShot(die);
    }
}
