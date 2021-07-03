using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    float timer;
    public GameObject meleeEnemyPrefab;
    public GameObject distanceEnemyPrefab;
    public Transform kevinPosition;

    public bool canSpawn;

    void Start()
    {
        canSpawn = false;
        timer = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                timer = 0;
                float x = Random.Range(-15f, 15f);
                float z = Random.Range(-15f, 15f);
                Vector3 position = kevinPosition.position + new Vector3(x, 0, z);
                Quaternion rotation = new Quaternion();

                float random = Random.Range(-1f, 1f);
                if (random < 0)
                {
                    GameObject go;
                    go = Instantiate(meleeEnemyPrefab, position, rotation) as GameObject;
                    go.GetComponent<NavMeshAgent>().Warp(position);
                }
                else if (random > 0)
                {
                    GameObject go;
                    go = Instantiate(distanceEnemyPrefab, position, rotation) as GameObject;
                    go.GetComponent<NavMeshAgent>().Warp(position);
                }
            }
        }
    }

    public void startSpawn()
    {
        canSpawn = false;
    }

    public void stopSpawn()
    {
        canSpawn = false;
    }
}
