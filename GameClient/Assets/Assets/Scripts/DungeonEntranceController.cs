using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntranceController : MonoBehaviour
{
    /*Singleton*/
    public static DungeonEntranceController dungeonEntranceController;

    // Start is called before the first frame update
    void Start()
    {
        if (dungeonEntranceController == null)
        {
            dungeonEntranceController = this;
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
            SceneManager.LoadScene("Dungeon");
            this.GetComponent<SphereCollider>().enabled = false;

            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        

    }

}
