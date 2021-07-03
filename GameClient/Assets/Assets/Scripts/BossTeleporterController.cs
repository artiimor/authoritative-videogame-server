using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTeleporterController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Kevin")
        {
            /*Hacemos que no pueda spawnear enemigos*/
            GameObject temp = GameObject.Find("scripter");
            temp.GetComponent<EnemySpawner>().stopSpawn();

            temp = GameObject.Find("Gate");
            Destroy(temp);

            /*Cargamos la escena*/
            SceneManager.LoadScene("Boss");
            this.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
