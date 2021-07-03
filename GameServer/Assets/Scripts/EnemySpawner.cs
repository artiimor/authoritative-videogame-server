using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    float timer;
    public GameObject meleeEnemyPrefab;
    public GameObject distanceEnemyPrefab;

    public bool canSpawn;
    private int maxEnemies = 10;
    private int enemyCount;

    void Start()
    {
        canSpawn = false;
        enemyCount = 0;
        timer = 34f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canSpawn)
        {
            timer += Time.deltaTime;

            if (timer >= 35f)
            {
                timer = 0;

                if (enemyCount >= maxEnemies)
                    return;

                float x = Random.Range(-5f, 5f);
                float z = Random.Range(-5f, 5f);
                Vector3 position = transform.position + new Vector3(x, 0, z);
                Quaternion rotation = new Quaternion();

                float random = Random.Range(-1f, 1f);

                if (random <= 0)//random < 0)
                {
                    GameObject go;
                    go = Instantiate(meleeEnemyPrefab, position, rotation) as GameObject;
                    go.GetComponent<NavMeshAgent>().Warp(position);
                    enemyCount++;

                    go.GetComponent<MeleeEnemy>().id = Enemy.maxId;
                    Enemy.maxId++;

                    // send it to the client
                    ServerSend.SpawnMeleeEnemy(go.GetComponent<MeleeEnemy>());
                }
                else if (random > 0)
                {
                    GameObject go;
                    go = Instantiate(distanceEnemyPrefab, position, rotation) as GameObject;
                    go.GetComponent<NavMeshAgent>().Warp(position);
                    enemyCount++;

                    go.GetComponent<DistanceEnemy>().id = Enemy.maxId;
                    Enemy.maxId++;

                    // send it to the client
                    ServerSend.SpawnDistanceEnemy(go.GetComponent<DistanceEnemy>());
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

    public void enemyKilled()
    {
        enemyCount--;
    }
}
