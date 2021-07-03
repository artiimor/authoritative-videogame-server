using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    /*Server*/
    public int id;

    /*Animaciones*/
    public Animator animator;

    /*Para relacionarnos con el jugador*/
    public Transform player;
    public PlayerCombat playerCombat;

    /*Para el movimiento*/
    public LayerMask whatIsGround, whatIsPlayer;
    public NavMeshAgent agent;

    protected bool dead = false;

    /*Para el estado de busqueda*/
    public Vector3 walkPoint;
    protected bool walkPointSet;
    public float walkPointRange;

    /*Para el estado de ataque*/
    protected bool alreadyAttacked;

    /*Parte principal de los estados de la IA*/
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    /*Gestores del propio enemigo*/
    public EnemyStats enemyStats;
    protected float dissapearTime = 10f;

    /*Para el loot*/
    public GameObject item;
    public GameObject potion;

    /*Para el knockback*/
    protected Rigidbody rb;
    protected bool knockBackCheck = false;
    protected bool stunned = false;
    public float stunTime = 2f; // Tiempo de duracion del stun
    protected float timer;

    /*Sonido*/
    protected AudioSource audioSource;
    public AudioClip searching;
    public AudioClip reee;
    public AudioClip attacking;
    public AudioClip getHurt;
    public AudioClip die;
    public AudioClip walk;
    public AudioClip run;

    /*Funcion de inicio*/
    void Start()
    {
        alreadyAttacked = false;

        // Jugador
        // GameObject temp = GameObject.Find("Kevin");
        // Transform kevinTransform = temp.GetComponent<Transform>();
        playerCombat = null;

        player = null;

        // movimiento
        agent = this.GetComponent<NavMeshAgent>();

        // Fisicas
        rb = this.GetComponent<Rigidbody>();

        // Audio
        audioSource = GetComponent<AudioSource>();
    }


    /*********************************************************************************/
    /**************************ESTADO DE BUSQUEDA AL JUGADOR**************************/
    /*********************************************************************************/
    protected virtual void Patroling()
    {
        /*Animaciones. no corre, patrulla*/
        animator.SetBool("Running", false);
        animator.SetBool("Patrolling", true);

        /*Andar de forma aleatoria*/
        /*Si no tiene direccion la buscamos*/
        /*if (!walkPointSet)
        {
            SearchWalkPoint();
        }*/

        /*Que ande hacia la direccion*/
        /*if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }*/

        /*Cuando llegue buscamos otro punto*/
        /*Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }*/
    }

    protected void SearchWalkPoint()
    {
        /*Obtenemos un punto aleatorio en el rango*/
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    /*********************************************************************************/
    /***************************ESTADO DE ATACAR AL JUGADOR***************************/
    /*********************************************************************************/
    protected virtual void AttackPlayer()
    {
        /*Se para y mira hacia el jugador*/
        agent.SetDestination(transform.position);
        transform.LookAt(player);
    }

    /*********************************************************************************/
    /**********************************ESTADO DE STUN*********************************/
    /*********************************************************************************/
    protected void Stunned()
    {
        /*Animaciones. Nos ponemos en idle*/
        animator.SetBool("Patrolling", false);
        animator.SetBool("Running", false);

        timer += Time.deltaTime;

        /*Quietito*/
        walkPoint = transform.position;

        /*Comprobamos si ha pasado el tiempo del stun*/
        if (timer >= stunTime)
        {
            stunned = false;
        }
    }

    /*Funciones para el knockback*/
    public void knockBack()
    {
        knockBackCheck = true;
    }

    public void getStun()
    {
        timer = 0f; // Reseteamos el timer
        agent.speed = 0f; // Quieto
        stunned = true; // Modo stun
    }

    /*Haber sime muero*/
    protected virtual void Die()
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

        /*Le hacemos desaparecer*/
        Invoke(nameof(remove), dissapearTime);

        /*Dropea compadre*/
        /* int n = Random.Range(1, 101);
        if (n < 30)
        {
            Instantiate(item, transform.position, transform.rotation);
        }
        if (n > 59)
        {
            Instantiate(potion, transform.position, transform.rotation);
        } */
    }

    protected void remove()
    {
        Destroy(gameObject);
    }

    /*Sonidos*/
    protected void playDeathSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(die);
    }

    protected void playAttackSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(attacking);
    }

    protected void playSearchSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(searching);
    }

    protected void playReeSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(reee);
    }

    protected void playWalkSound()
    {
        audioSource.PlayOneShot(walk);
    }

    protected void playRunSound()
    {
        audioSource.PlayOneShot(run);
    }

    /*New server shit*/
    public void UpdateTargetPlayer(PlayerManager _player) 
    {
        player = _player.transform;
        playerCombat = _player.gameObject.GetComponent<PlayerCombat>();
    }

    public void RemoveTargetPlayer()
    {
        player = null;
        playerCombat = null;
    }

    public void UpdateEnemyPosition(Vector3 _position)
    {
        try
        {
            agent.SetDestination(_position);
        } catch (Exception _ex)
        {
            Debug.Log("" + _ex);
            agent = new NavMeshAgent();
        }
    }

    public virtual void AttackReceived()
    {

    }
}
