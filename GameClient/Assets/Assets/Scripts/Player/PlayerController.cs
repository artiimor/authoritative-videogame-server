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
    public PlayerCombat playerCombat;
    public InventoryManager inventoryManager;


    public Transform targetEnemy = null;

    // Animaciones
    public Animator animator;

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
            /*Funcionalidad de movernos*/
            if (Input.GetMouseButton(0))
            {
                // Debug.Log("PlayerController.eso");
                playerMovement.getPosition();
            }

            /*Funcionalidad de ataque a melee*/
            if (Input.GetMouseButtonDown(1))
            {
                playerCombat.Attack();
            }

            /*Funcionalidad de ataque a distancia*/
            if (Input.GetKeyDown(KeyCode.Q))
            {
                playerCombat.DistanceAttack();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                // playerCombat.areaAttack();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Debug.Log("Proteccion :" + this.GetComponent<InventoryManager>().getProteccionMod());
                // Debug.Log("Damage :" + this.GetComponent<InventoryManager>().getDamageMod());
            }

            /*Funcionalidad de curación*/
            if (Input.GetKeyDown(KeyCode.P))
            {
                // Debug.Log("Pulsada p");
                if(inventoryManager.pocionesDisponibles())
                    ClientSend.PlayerRestoreHealth();
            }


            /*Comprobacion para hacer el ataque mas intuitivo*/
            if (targetEnemy != null)
            {
                // Debug.Log("Distancia con el enemigo: " + Vector3.Distance(transform.position, targetEnemy.position));
                if (Vector3.Distance(transform.position, targetEnemy.position) < 1)
                {
                    playerCombat.hitEnemies();
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
    }
}
