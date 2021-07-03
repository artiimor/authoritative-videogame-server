using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChurchExitController : MonoBehaviour
{
    bool active = false;

    void OnTriggerEnter(Collider other)
    {
        // gameObject.SetActive(false);
        if (active)
        {
            SceneManager.LoadScene("Tutorial");

            GameObject temp = GameObject.Find("scripter");
            temp.GetComponent<EnemySpawner>().startSpawn();

            /*Le permitimos actualizar el score*/
            temp = GameObject.Find("scripter");
            temp.GetComponent<ScoreManager>().updateData();
        }

    }

    void OnTriggerExit(Collider other)
    {
        active = true;
    }
}
