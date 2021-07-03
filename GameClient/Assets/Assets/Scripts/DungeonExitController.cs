using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExitController : MonoBehaviour
{
    bool active = false;

    void OnTriggerEnter(Collider other)
    {
        // Debug.Log(active + "");
        // gameObject.SetActive(false);
        if (active)
        {
            /*Hacemos que pueda spawnear enemigos*/
            GameObject temp = GameObject.Find("scripter");
            temp.GetComponent<EnemySpawner>().startSpawn();

            /*Le permitimos actualizar el score*/
            // temp.GetComponent<ScoreManager>().updateData();

            SceneManager.LoadScene("Tutorial");
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        active = true;
    }
}
