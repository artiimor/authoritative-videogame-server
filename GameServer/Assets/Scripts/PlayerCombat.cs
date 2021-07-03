using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerCombat : MonoBehaviour
{
    // Animaciones
    public Animator animator;

    // Interaccion
    private Vector3 targetPosition;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float soundRange = 5f;
    public LayerMask enemy;
    public float attackDamage = 20f;

    public PlayerMovement playerMovement;
    public PlayerController playerController;
    public PlayerCooldown playerCooldown;
    private Transform attackingEnemy;
    public InventoryManager inventoryManager;
    

    // Ataque a distancia
    public GameObject projectile;
    public Transform playerTransform;

    // Ataque especial
    public GameObject trumpetEffect;

    public float health = 100f;
    private float maxHealth = 100f;
    // public HealthbarController hbController;

    // Sonidos
    private AudioSource audioSource;
    public AudioClip swordImpact;
    public AudioClip magicCast;
    public AudioClip areaCast;

    /*Singleton*/
    public static PlayerCombat playerCombat;

    // Efectos del jefe
    bool isPoisoned = false;
    float poisonDamage = 2f;
    float poisonTimer = 5f;

    float slowTimer = 5f;

    void Start()
    {
        /*Instanciacion del singleton*/
        /*if (playerCombat == null)
        {
            audioSource = GetComponent<AudioSource>();
            playerCombat = this;
        }
        else
        {
            Destroy(this);
            DontDestroyOnLoad(this);
        }*/
    }

    public void Attack()
    {
        // Comprobamos que el cooldown ha pasado
        if (playerCooldown.canAttack())
        {
            // Rotamos al personaje hacia donde estamos mirando
            getAttackPosition();

            // Si le ha dado a un enemigo
            if (attackingEnemy != null)
            {
                // Vemos lo lejos que esta
                float distance = Vector3.Distance(attackingEnemy.position, transform.position);
                playerController.targetEnemy = attackingEnemy;

                playerMovement.enemyTransform = attackingEnemy;

                // Si no le podemos atacar nos acercamos. 2.5 es un factor de conversion.
                // TODO arreglar el 2.5
                if (distance > attackRange * 2.5)
                {
                    return;
                }

            }

            hitEnemies();
        }

    }

    private void playSwordSound()
    {
        audioSource.PlayOneShot(swordImpact);
    }

    private void playAreaSound()
    {
        audioSource.PlayOneShot(areaCast);
    }

    private void playSpellSound()
    {
        audioSource.PlayOneShot(magicCast);
    }

    public void hitEnemies()
    {
        // Hacemos la animacion
        animator.SetTrigger("Attack");
        animator.SetBool("Walking", false);

        // Sonido
        playSwordSound();

        // Obtenemos los enemigos a los que se ha golpeado
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        double mod = inventoryManager.getDamageMod();
        // Les quitamos la vida
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                // enemy.GetComponent<MeleeEnemy>().KnockBack();
                if (enemy.name == "Spider_Flower")
                {
                    // BossStats bossStats = enemy.GetComponent<BossStats>();
                    // bossStats.TakeDamage(attackDamage * (float)mod);
                }
                else
                {
                    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                    enemyStats.TakeDamage(attackDamage * (float)mod);
                }
            }
        }

        playerController.attackPending = false;
        attackingEnemy = null;
        playerController.targetEnemy = null;
        playerCooldown.updateLastAttack();

        playerMovement.enemyTransform = null;
        playerMovement.setPosition(transform.position);
    }

    public void DistanceAttack()
    {

        if (playerCooldown.canAttack())
        {
            // Detenemos el movimiento
            playerMovement.getPlayerPosition();

            // Hacemos la animacion
            animator.SetTrigger("Attack");
            animator.SetBool("Walking", false);

            GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            // Sonido
            playAreaSound();

            /*Para evitar que tengamos infinitas flechas en el juego de manera simultanea*/
            Destroy(arrow, 3f);

            // Rotamos al personaje hacia donde estamos mirando
            getAttackPosition();

            arrow.transform.eulerAngles = new Vector3(0f, playerTransform.eulerAngles.y, 0f);
            arrow.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z) + (playerTransform.forward * 0.5f) ;

            rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
            rb.AddForce(transform.up * 5f, ForceMode.Impulse);

            /*Reset del cooldown*/
            playerController.attackPending = false;
            attackingEnemy = null;
            playerController.targetEnemy = null;
            playerCooldown.updateLastAttack();

            playerMovement.enemyTransform = null;
            playerMovement.setPosition(transform.position);
        }
    }

    public void areaAttack()
    {
        if (playerCooldown.canAttack())
        {
            // Hacemos la animacion
            animator.SetTrigger("Attack");
            animator.SetBool("Walking", false);


            // Obtenemos los enemigos a los que se ha golpeado
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, soundRange, enemy);

            // spawneamos el efecto
            GameObject effect = Instantiate(trumpetEffect, transform.position, Quaternion.Euler(-90f, 0, 0));
            /*Aunque se acabe y no sea loopeable el efecto sigue existiendo*/
            Destroy(effect, 2f);

            // Sonido
            playSpellSound();

            /*Reset del cooldown*/
            playerController.attackPending = false;
            attackingEnemy = null;
            playerController.targetEnemy = null;
            playerCooldown.updateLastAttack();

            playerMovement.enemyTransform = null;
            playerMovement.setPosition(transform.position);

            // Les empujamos
            foreach (Collider enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    Debug.Log(enemy.name);
                    if (enemy.name == "meleeEnemyPrefab(Clone)")
                    {
                        // Pa atras manin
                        MeleeEnemy meleeEnemy = enemy.GetComponent<MeleeEnemy>();
                        meleeEnemy.getStun();
                        meleeEnemy.knockBack();
                    }
                    else if (enemy.name == "distanceEnemyPrefab(Clone)")
                    {
                        // Pa atras manin
                        DistanceEnemy distanceEnemy = enemy.GetComponent<DistanceEnemy>();
                        distanceEnemy.getStun();
                        distanceEnemy.knockBack();
                    }
                    else if (enemy.name == "Spider_Flower")
                    {
                        // Pa atras manin
                        // BossController boss = enemy.GetComponent<BossController>();
                        // boss.getStun();
                        // boss.knockBack();
                    }
                }
            }

            
        }
    }

    private void getAttackPosition()
    {

        // Obtenemos la posicion a la que mira
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = new Vector3(hit.point.x, 0, hit.point.z);
        }

        // Cambiamos la rotacion
        Quaternion newRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.forward);

        newRotation.x = 0f;
        newRotation.z = 0f;

        transform.rotation = newRotation;

        // Comprobamos si le ha dado a un enemigo
        if (hit.collider.gameObject.tag == "Enemy")
        {
            attackingEnemy = hit.collider.transform;
        }
    }

    public void getDamage(float damage)
    {
        double mod = inventoryManager.getProteccionMod();
        health -= damage * (float)mod;
        // hbController.SetHealth(health);

        ServerSend.UpdatePlayerHealth(this.gameObject.GetComponent<Player>().id, health);

        if (health <= 0)
        {
            Die();
        }
    }

    public float getHealth()
    {
        return health;
    }

    public void restoreHealth(float restoration)
    {
        health = (health + restoration);
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        // ServerSend.UpdatePlayerHealth(this.gameObject.GetComponent<Player>().id, health);
        // hbController.SetHealth(health);
    }

    /*Efectos del jefe*/
    /*Veneno*/
    public void getPoisoned()
    {
        isPoisoned = true;
        StartCoroutine(PoisonDamage());
    }

    IEnumerator PoisonDamage()
    {
        // Debug.Log("Recibiendo damage o algo");
        getDamage(poisonDamage);
        yield return new WaitForSeconds(poisonTimer);

        if (isPoisoned)
        {
            StartCoroutine(PoisonDamage());
        }
    }

    /*Ralentizacion*/
    public void getSlowDown()
    {
        StartCoroutine(applySlowDown());
    }

    IEnumerator applySlowDown()
    {
        playerMovement.agent.speed = playerMovement.agent.speed / 2;
        yield return new WaitForSeconds(slowTimer);
        playerMovement.agent.speed = playerMovement.agent.speed * 2;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(attackPoint.position, soundRange);
    }

    void Die()
    {
        Debug.Log("AHHHHH, LA PALMASTE :((((((");
    }



}
