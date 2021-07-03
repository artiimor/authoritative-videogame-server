using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();
    public static Dictionary<int, ItemController> items = new Dictionary<int, ItemController>();

    // Players
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    
    // Enemies
    public GameObject meleeEnemyPrefab;
    public GameObject distanceEnemyPrefab;

    // Items
    public GameObject potionPrefab;
    public GameObject axePrefab;
    public GameObject milkPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); // to keep gamemanager
        }
        else if (instance != this)
        {
            // Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _position.y = 0;
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
            CameraController.cameraController.playerPosition = _player.transform;
        }
        else
        {
            // Debug.Log("Instanciando variante de jugador");
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnMeleeEnemy(int _id, Vector3 _position, Quaternion _rotation)
    {
        Debug.Log("Recibido spawn");
        if (enemies.ContainsKey(_id))
        {
            Debug.Log("Clave repetida");
            // return;
            // Destroy(enemies[_id].gameObject);
            enemies.Remove(_id);
            // return;
        }

        GameObject _enemy;
        _position.y = 0;
        _enemy = Instantiate(meleeEnemyPrefab, _position, _rotation);

        _enemy.GetComponent<MeleeEnemy>().id = _id;
        enemies.Add(_id, _enemy.GetComponent<MeleeEnemy>());
    }

    public void SpawnDistanceEnemy(int _id, Vector3 _position, Quaternion _rotation)
    {
        Debug.Log("Recibido spawn");
        if (enemies.ContainsKey(_id))
        {
            Debug.Log("Clave repetida");
            // return;
            // Destroy(enemies[_id].gameObject);
            enemies.Remove(_id);
        }

        GameObject _enemy;
        _position.y = 0;
        _enemy = Instantiate(distanceEnemyPrefab, _position, _rotation);

        _enemy.GetComponent<DistanceEnemy>().id = _id;
        enemies.Add(_id, _enemy.GetComponent<DistanceEnemy>());
    }

    public void UpdatePlayerPosition(int _id, Vector3 _newPosition)
    {
        try
        {
            // Only update if the player is not out character
            if (playerPrefab.GetComponent<PlayerManager>().id != _id)
            {
                players[_id].gameObject.GetComponent<PlayerMovement>().position = _newPosition;
                players[_id].gameObject.GetComponent<PlayerMovement>().agent.SetDestination(_newPosition);
            }
            
        } catch (Exception _ex)
        {
            // Debug.Log($"Couldn't update player position {_ex}");
        }
    }

    public void SpawnPotion(int _id, Vector3 _position)
    {
        if (items.ContainsKey(_id))
        {
            return;
        }
        GameObject _item;
        _item = Instantiate(potionPrefab, _position, Quaternion.identity);
        _item.GetComponent<ItemController>().id = _id;

        items.Add(_id, _item.GetComponent<ItemController>());
    }

    public void SpawnAxe(int _id, Vector3 _position)
    {
        if (items.ContainsKey(_id))
        {
            return;
        }
        GameObject _item;
        _item = Instantiate(axePrefab, _position, Quaternion.identity);
        _item.GetComponent<ItemController>().id = _id;

        items.Add(_id, _item.GetComponent<ItemController>());
    }

    public void SpawnMilk(int _id, Vector3 _position)
    {
        if (items.ContainsKey(_id))
        {
            return;
        }
        GameObject _item;
        _item = Instantiate(milkPrefab, _position, Quaternion.identity);
        _item.GetComponent<ItemController>().id = _id;

        items.Add(_id, _item.GetComponent<ItemController>());
    }
}
