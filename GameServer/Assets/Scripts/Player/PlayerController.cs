using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    

    // Interaccion con el mundo
    private Vector3 targetPosition;
    public bool attackPending;
    public PlayerMovement playerMovement;
    public Player player;
    public bool connected = true;
    // public PlayerCombat playerCombat;
    // public InventoryManager inventoryManager;


    public Transform targetEnemy = null;

    // Animaciones
    // public Animator animator;

    // fix para los jugadores no locales
    public bool isLocal = true;

    /*Singleton*/
    public static PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement.getPlayerPosition();

        DontDestroyOnLoad(this.gameObject);

        /*Instanciacion del singleton*/
        /*if (playerController == null)
        {
            playerController = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

        }*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocal)
        {
            /*Comprobacion para hacer el ataque mas intuitivo*/
            if (targetEnemy != null)
            {
                if (Vector3.Distance(transform.position, targetEnemy.position) < 1)
                {
                    player.hitEnemies();
                    playerMovement.getPlayerPosition();
                    targetEnemy = null;
                }
                else
                {
                    playerMovement.position = targetEnemy.position;
                }
            }
        }

        /*movimiento*/
        if (playerMovement != null)
            playerMovement.movePlayer();

        if (!connected)
        {
            Destroy(this.gameObject);
        }
    }
}
