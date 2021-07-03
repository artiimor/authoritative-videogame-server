using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    // Movimiento
    public float speed = 5f; // Velocidad a la que anda el personaje
    // public CharacterController playerController;
    public Vector3 position; // Posicion actual del personaje
    public Transform enemyTransform;

    public NavMeshAgent agent;

    /*Singleton*/
    // public static PlayerMovement playerMovement;

    private NavMeshPath navMeshPath;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        navMeshPath = new NavMeshPath();

        /*Instanciacion del singleton*/
        /* if (playerMovement == null)
        {
            playerMovement = this;
        }
        else
        {
            Destroy(this);
        }*/
    }

    private void FixedUpdate()
    {
        if (enemyTransform != null)
        {
            agent.SetDestination(enemyTransform.position);
        }
    }

    // Obtiene la función en la que se encuentra el personaje y lo convierte en la posicion actual
    public void getPlayerPosition()
    {
        position = transform.position;
    }


    // Obtiene la función en la que se ha clickado
    public void getPosition()
    {
        Debug.Log("getPosition en PlayerMovement");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.Log("antes del condicional");
        if (Physics.Raycast(ray, out hit, 1000))
        {
            Debug.Log("condicional");
            Vector3 _position = new Vector3(hit.point.x, 0, hit.point.z);
            this.position = _position;

            // change movement. First notify to the server
            Debug.Log("Enviando el vector " + _position);
            agent.SetDestination(_position);
            // ClientSend.PlayerMovement(_position);
        }
    }

    public void setPosition(Vector3 position)
    {
        agent.SetDestination(position);
        this.position = position;
    }

    // Mueve al jugador
    public void movePlayer()
    {
        
        if (Vector3.Distance(transform.position, position) > 1)
        {
            // Lo movemos
            
            Quaternion newRotation = Quaternion.LookRotation(position - transform.position, Vector3.forward);

            newRotation.x = 0f;
            newRotation.z = 0f;


            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 50f);
            // playerController.SimpleMove(transform.forward * speed);
            
        }
    }
}
