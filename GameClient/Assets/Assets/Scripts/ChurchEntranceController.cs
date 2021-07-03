using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChurchEntranceController : MonoBehaviour
{
    /*Singleton*/
    public static ChurchEntranceController churchEntranceController;

    // Start is called before the first frame update
    void Start()
    {
        if (churchEntranceController == null)
        {
            churchEntranceController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Kevin")
        {
            /*Hacemos que pueda spawnear enemigos*/
            GameObject temp = GameObject.Find("scripter");
            temp.GetComponent<EnemySpawner>().stopSpawn();

            /*Cargamos la escena*/
            SceneManager.LoadScene("Church");
            this.GetComponent<SphereCollider>().enabled = false;

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}
