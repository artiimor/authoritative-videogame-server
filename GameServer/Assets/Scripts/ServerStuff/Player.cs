using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public Vector3 targetPosition;
    public NavMeshAgent agent;

    public float health = 100f;
    private float maxHealth = 100f;

    /*Inventory stuff*/
    public int nPotions = 0;
    public int nMilk = 0;
    public int nAxes = 0;
    public float potionRestoration = 20f;

    /*Combate*/
    private Transform attackingEnemy;
    public float attackDamage = 20f;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemy;

    /*Distance combat*/
    public GameObject projectile;
    public Transform playerTransform;
    public float kunaiDamage = 25f;

    /*Other scripts*/
    public PlayerCooldown playerCooldown;
    public PlayerController playerController;
    public PlayerMovement playerMovement;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
    }

    public void SetPosition(Vector3 _newPosition)
    {
        // TODO: diferenciar posicion real del target
        agent.SetDestination(_newPosition);
        targetPosition = _newPosition;

        ServerSend.PlayerPosition(this);
    }

    public void getDamage(float damage)
    {
        double mod = getProteccionMod();
        health -= damage * (float)mod;

        Debug.Log("HEALTH of player" + id + " : " + health);
        ServerSend.PlayerHurt(id, health);

        if (health <= 0)
        {
            // TODO handle death
            Die();
        }
    }

    private double getProteccionMod()
    {
        return 1 - (nMilk * 0.1);
    }

    public void addPocion()
    {
        nPotions++;
    }

    public void addDamage()
    {
        nAxes++;
    }

    public void addProteccion()
    {
        nMilk++;
    }

    public void Attack(Vector3 _attackPosition, Quaternion _rotation)
    {
        transform.rotation = _rotation;
        // Comprobamos que el cooldown ha pasado
        // attackPoint.position = _attackPosition;
        if (playerCooldown.canAttack())
        {
            // Rotamos al personaje hacia donde estamos mirando
            // getAttackPosition() hecho en el cliente

            // Si le ha dado a un enemigo
            if (attackingEnemy != null)
            {
                // Vemos lo lejos que esta
                float distance = Vector3.Distance(attackingEnemy.position, transform.position);
                playerController.targetEnemy = attackingEnemy;

                playerMovement.enemyTransform = attackingEnemy;

                // Si no le podemos atacar nos acercamos. 2.5 es un factor de conversion.
                if (distance > attackRange * 2.5)
                {
                    return;
                }
            }

            /*Caso en que atacamos al aire*/

            hitEnemies();
        }
    }

    public void Attack()
    {
        // Comprobamos que el cooldown ha pasado
        // attackPoint.position = _attackPosition;
        if (playerCooldown.canAttack())
        {
            // Rotamos al personaje hacia donde estamos mirando
            // getAttackPosition() hecho en el cliente

            // Si le ha dado a un enemigo
            if (attackingEnemy != null)
            {
                // Vemos lo lejos que esta
                float distance = Vector3.Distance(attackingEnemy.position, transform.position);
                playerController.targetEnemy = attackingEnemy;

                playerMovement.enemyTransform = attackingEnemy;

                // Si no le podemos atacar nos acercamos. 2.5 es un factor de conversion.
                if (distance > attackRange * 2.5)
                {
                    return;
                }
            }

            /*Caso en que atacamos al aire*/

            hitEnemies();
        }
    }

    public void hitEnemies()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemy);
        double mod = 1f + nAxes * 0.1f;
        // Les quitamos la vida
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                // enemy.GetComponent<MeleeEnemy>().KnockBack();
                EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(attackDamage * (float)mod);
            }
        }

        playerController.attackPending = false;
        attackingEnemy = null;
        playerController.targetEnemy = null;
        playerCooldown.updateLastAttack();

        playerMovement.enemyTransform = null;
        playerMovement.setPosition(transform.position);
    }

    public void AttackEnemy(int enemyId)
    {
        Enemy[] enemies = (Enemy[])FindObjectsOfType(typeof(Enemy));

        foreach (Enemy enemy in enemies)
        {
            if (enemy.id == enemyId)
            {
                attackingEnemy = enemy.transform;
                Attack();
                break;
            }
        }
    }

    public void DistanceAttack(Quaternion _rotation)
    {
        // rotar al personaje
        transform.rotation = _rotation;
        transform.rotation = _rotation;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 50f, enemy))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            EnemyStats enemy = hit.transform.gameObject.GetComponent<EnemyStats>();
            enemy.TakeDamage(kunaiDamage);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        // Instanciar flecha igual que en el cliente
        // GameObject arrow = Instantiate(projectile, transform.position, Quaternion.identity);
        // Debug.Log("Posicion de instancia: " + transform.position);
        // Rigidbody rb = arrow.GetComponent<Rigidbody>();
        /*Para evitar que tengamos infinitas flechas en el juego de manera simultanea*/
        // Destroy(arrow, 3f);
        // arrow.transform.eulerAngles = new Vector3(0f, playerTransform.eulerAngles.y, 0f);
        // arrow.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z) + (playerTransform.forward * 0.5f);

        // rb.AddForce(transform.forward * 28f, ForceMode.Impulse);
        // rb.AddForce(transform.up * 5f, ForceMode.Impulse);

    }

    public void RestoreHealth()
    {
        // Debug.Log("Comprobación pociones");
        if (nPotions > 0)
        {
            // Debug.Log("current health: " + health);
            health += potionRestoration;
            // Debug.Log("Una vez curados: " + health);
            ServerSend.PlayerHurt(id, health); // Update health
            ServerSend.ConsumePotion(id); // consume potion

            nPotions--;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }

    public void Disconnect()
    {
        playerController.connected = false;
    }

    public void Die()
    {
        ServerSend.PlayerDisconnect(this.id);

        Destroy(this.gameObject);
    }
}