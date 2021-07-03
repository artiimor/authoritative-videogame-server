using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        string _publicKey = _packet.ReadString();

        // Debug.Log($"Message from server: {_msg}");
        // Debug.Log($"Server public key: {_publicKey}");
        RSA.addPublicKey(_publicKey); // add public key to encrypt login

        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void WrongLogin(Packet _packet)
    {
        string _msg = _packet.ReadString();

        Debug.Log($"Message from server: {_msg}");

        UIManager aux = FindObjectOfType<UIManager>();
        if (aux != null)
        {
            aux.WrongLogIn();
        }
    }

    public static void CorrectLogin(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _msg = _packet.ReadString();

        Debug.Log($"Message from server: {_msg}");

        Client.instance.playerId = _id;
        Client.instance.username = _username;

        ClientSend.LoginReceived();

        UIManager.getInstance().DontShowMenu();
    }

    public static void CorrectSignUp(Packet _packet)
    {
        Debug.Log("Correct Sign Up");
    }

    public static void WrongSignUp(Packet _packet)
    {
        Debug.Log("Wrong Sign Up");

        UIManager aux = FindObjectOfType<UIManager>();
        if (aux != null)
        {
            aux.WrongSingIn();
        }
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        
        /*Cargamos la escena*/
        SceneManager.LoadScene("Tutorial");

        ClientSend.InGame();
    }

    public static void UpdatePosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _newPosition = _packet.ReadVector3();

        GameManager.instance.UpdatePlayerPosition(_id, _newPosition);
    }

    public static void SpawnMeleeEnemy(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnMeleeEnemy(_id, _position, _rotation);

        // Debug.Log("Spawning melee enemy");
    }

    public static void SpawnDistanceEnemy(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnDistanceEnemy(_id, _position, _rotation);

        // Debug.Log("Spawning distance enemy");
    }

    public static void UpdateEnemyTarget(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        int _playerId = _packet.ReadInt();

        GameManager.enemies[_enemyId].UpdateTargetPlayer(GameManager.players[_playerId]);
    }

    public static void RemoveEnemyTarget(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();

        GameManager.enemies[_enemyId].RemoveTargetPlayer();
    }
    public static void UpdateEnemyPosition(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        // // Debug.Log("enemyID : " + _enemyId);
        // // Debug.Log("DIC: " + GameManager.enemies);

        GameManager.enemies[_enemyId].UpdateEnemyPosition(_position);
    }

    public static void MeleeEnemyAttack(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();

        GameManager.enemies[_enemyId].AttackReceived();
    }

    public static void DistanceEnemyAttack(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();

        GameManager.enemies[_enemyId].AttackReceived();
    }

    public static void UpdatePlayerHealth(Packet _packet)
    {
        float _playerHealt = _packet.ReadFloat();

        // TODO: end this
    }

    public static void PlayerAttackTrigger(Packet _packet)
    {
        int _player = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_player].GetComponent<Animator>().SetTrigger("Attack");
        GameManager.players[_player].transform.rotation = _rotation;
    }

    public static void PlayerAttackEnemy(Packet _packet)
    {
        int _player = _packet.ReadInt();
        int _enemy = _packet.ReadInt();

        GameManager.players[_player].GetComponent<PlayerCombat>().attackEnemy(_enemy);
    }

    public static void EnemyHurt(Packet _packet)
    {
        Debug.Log("Enemigo damageado");
        int _enemy = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.enemies[_enemy].GetComponent<EnemyStats>().setHealth(_health);
    }

    public static void PlayerDisconnect(Packet _packet)
    {
        int _player = _packet.ReadInt();

        // Just remove it :D
        Destroy(GameManager.players[_player].gameObject);
        GameManager.players.Remove(_player);

        Debug.Log("Player: " + _player);
        Debug.Log("myId: " + Client.instance.myId);
        if (_player == Client.instance.myId)
        {
            Debug.Log("Quitando aplicación");
            Application.Quit();
        }
    }

    public static void PlayerDistanceAttack(Packet _packet)
    {
        int _player = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_player].GetComponent<PlayerCombat>().DistanceAttack(_rotation);
    }

    public static void PlayerHurt(Packet _packet)
    {
        int _player = _packet.ReadInt();
        Debug.Log("_player: " + _player);
        Debug.Log("Me: " + Client.instance.myId);
        if (_player != Client.instance.myId)
            return;

        float _health = _packet.ReadFloat();

        // Debug.Log("new health: " + _health);

        GameManager.players[Client.instance.myId].GetComponent<PlayerCombat>().setHealth(_health);
    }

    public static void PotionAdded(Packet _packet) 
    {
        GameManager.players[Client.instance.myId].GetComponent<InventoryManager>().addPocion();
    }

    public static void SpawnPotion(Packet _packet) 
    {
        int _itemId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        Debug.Log("Adding potion with id: " + _itemId);
        GameManager.instance.SpawnPotion(_itemId, _position);
    }

    public static void SpawnAxe(Packet _packet) 
    {
        int _itemId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.instance.SpawnAxe(_itemId, _position);
    }

    public static void SpawnMilk(Packet _packet) 
    {
        int _itemId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

       GameManager.instance.SpawnMilk(_itemId, _position);
    }

    public static void DestroyItem(Packet _packet)
    {
        int itemId = _packet.ReadInt();

        Destroy(GameManager.items[itemId].gameObject);
    }

    public static void ConsumePotion(Packet _packet)
    {
        GameManager.players[Client.instance.myId].GetComponent<InventoryManager>().delPocion();
    }
}
