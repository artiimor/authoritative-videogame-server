using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
    }

    public static void Login(int _fromClient, Packet _packet)
    {
        // int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _password = _packet.ReadString();

        // Debug.Log("Usuario: " + _username + "\nPassword: " + _password);

        // Check user from our database
        if (DatabaseManager.authenticateUser(_username, _password))
        {
            if (Server.clientsLogued.Contains(_username))
            {
                ServerSend.WrongLogin(_fromClient);
            }
            else
            {
                Server.clientsLogued.Add(_username);
                // Server.clientsLoguedIds.Add(_fromClient);
                Server.clientsIds.Add(_fromClient, _username);
                ServerSend.CorrectLogin(_fromClient, 0, _username);
            }
        }
        else
        {
            ServerSend.WrongLogin(_fromClient);
        }
    }

    public static void LoginReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        int _clientGameId = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient} ({_clientGameId} in database).");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }

        Debug.Log($"Login received: {_clientIdCheck} {_clientGameId} {_username}");

        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        Vector3 _newPosition = _packet.ReadVector3();

        Server.clients[_fromClient].player.SetPosition(_newPosition);
    }

    public static void SendEnemiesData(int _fromClient, Packet _packet)
    {
        Server.clients[_fromClient].SendEnemiesData(_fromClient);
        Server.clients[_fromClient].SendItemsData(_fromClient);

        Server.clientsLoguedIds.Add(_fromClient);
    }

    public static void PlayerAttack(int _fromClient, Packet _packet)
    {
        // Debug.Log("Recibido attack sin más");

        Vector3 _attackPosition = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        /*Calculate attack*/
        Server.clients[_fromClient].player.Attack(_attackPosition, _rotation);

        /*Notify other players Only trigger attack*/
        ServerSend.PlayerAttackTrigger(_fromClient, _rotation);
    }

    public static void PlayerAttackEnemy(int _fromClient, Packet _packet)
    {
        // Debug.Log("Recibido attack a un enemigo");

        int _enemyId = _packet.ReadInt();

        /*Calculate attack*/
        Server.clients[_fromClient].player.AttackEnemy(_enemyId);

        /*Notify other players. just tell the enemy id*/
        ServerSend.PlayerAttackEnemy(_fromClient, _enemyId);
    }

    public static void PlayerDistanceAttack(int _fromClient, Packet _packet)
    {
        Quaternion _rotation = _packet.ReadQuaternion();

        /*Calculate attack*/
        Server.clients[_fromClient].player.DistanceAttack(_rotation);

        /*Notify other players*/
        ServerSend.PlayerDistanceAttack(_fromClient, _rotation);
    }

    public static void PlayerRestoreHealth(int _fromClient, Packet _packet)
    {
        // Debug.Log("Recuperando salud o algo asi");
        Server.clients[_fromClient].player.RestoreHealth();
    }

    public static void SignUp(int _fromClient, Packet _packet)
    {
        // int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _email = _packet.ReadString();
        string _password = _packet.ReadString();

        // Debug.Log("Usuario: " + _username + "\nPassword: " + _password);

        // Check user from our database
        if (DatabaseManager.insertUser(_username, _email, _password))
        {
            ServerSend.CorrectInsert(_fromClient);
            // Debug.Log("User authenticated :)))");
        }
        else
        {
            ServerSend.WrongInsert(_fromClient);
            // Debug.Log("wrong user data");
        }
    }
}
