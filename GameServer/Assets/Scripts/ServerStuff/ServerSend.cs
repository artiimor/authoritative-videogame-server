using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAllLogued(Packet _packet)
    {
        _packet.WriteLength();
        foreach (int i in Server.clientsLoguedIds)
        {
            Debug.Log("Cliente logueado: " + i);
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();

        foreach (int i in Server.clientsLoguedIds)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);
            _packet.Write(RSA.getPublicKey()); // add public key to Welcome package

            SendTCPData(_toClient, _packet);
        }
    }

    public static void CorrectLogin(int _toClient, int _id, string _username)
    {
        using (Packet _packet = new Packet((int)ServerPackets.correctLogin))
        {
            _packet.Write(_id);
            _packet.Write(_username);
            _packet.Write("[OK] Login successful");

            SendTCPData(_toClient, _packet);
        }
    }

    public static void WrongLogin(int _toClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.wrongLogin))
        {
            _packet.Write("[ERROR] invalid credentials");

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);

            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet); // We don't want to lose this packet >:D
        }
    }

    public static void SendEnemiesData(int _toClient)
    {
        Debug.Log("Enviándole los enemigos al jugador con id: " + _toClient);

        /*Now we notify every enemy to this player*/
        foreach (Enemy _enemy in GameObject.FindObjectsOfType<MeleeEnemy>())
        {
            SpawnMeleeEnemy(_toClient, _enemy);

            /*If enemy hasn't target, then update goal*/
            if (_enemy.player != null && _enemy.playerComponent != null)
            {
                EnemyTarget(_toClient, _enemy, _enemy.playerComponent);
            }
            else /*If enemy has player target, update it*/
            {
                UpdateEnemyPosition(_toClient, _enemy, _enemy.agent.destination);
            }
        }

        foreach (Enemy _enemy in GameObject.FindObjectsOfType<DistanceEnemy>())
        {
            SpawnDistanceEnemy(_toClient, _enemy);

            /*If enemy hasn't target, then update goal*/
            if (_enemy.player != null && _enemy.playerComponent != null)
            {
                EnemyTarget(_toClient, _enemy, _enemy.playerComponent);
            }
            else /*If enemy has player target, update it*/
            {
                UpdateEnemyPosition(_toClient, _enemy, _enemy.agent.destination);
            }
        }
    }

    public static void SendItemsData(int _toClient)
    {
        /*Now we notify every item to this player*/
        foreach (ItemContoller _item in GameObject.FindObjectsOfType<PotionController>())
        {
            SpawnPotion(_toClient, _item.transform.position);
        }

        foreach (ItemContoller _item in GameObject.FindObjectsOfType<MilkController>())
        {
            SpawnMilk(_toClient, _item.transform.position);
        }

        foreach (ItemContoller _item in GameObject.FindObjectsOfType<AxeController>())
        {
            SpawnAxe(_toClient, _item.transform.position);
        }
    }

    public static void SpawnMeleeEnemy(Enemy _enemy)
    {
        Debug.Log("Enviando spawn enemigo");
        using (Packet _packet = new Packet((int)ServerPackets.spawnMeleeEnemy))
        {
            _packet.Write(_enemy.id);
            // Debug.Log("Enviando paquete de spawneo melee enemy. ID = " + _enemy.id);

            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            SendTCPDataToAllLogued(_packet); // We don't want to lose this packet >:D
        }
    }

    public static void SpawnMeleeEnemy(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnMeleeEnemy))
        {
            _packet.Write(_enemy.id);
            // Debug.Log("Enviando paquete de spawneo melee enemy. ID = " + _enemy.id);

            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            SendTCPData(_toClient, _packet); // We don't want to lose this packet >:D
        }
    }

    public static void SpawnDistanceEnemy(Enemy _enemy)
    {
        Debug.Log("Enviando spawn enemigo");
        using (Packet _packet = new Packet((int)ServerPackets.spawnDistanceEnemy))
        {
            _packet.Write(_enemy.id);

            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            SendTCPDataToAllLogued(_packet); // We don't want to lose this packet >:D
        }
    }

    public static void SpawnDistanceEnemy(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnDistanceEnemy))
        {
            _packet.Write(_enemy.id);

            _packet.Write(_enemy.transform.position);
            _packet.Write(_enemy.transform.rotation);

            Debug.Log("Enviando enemigo al jugador con id: " + _toClient);
            SendTCPData(_toClient, _packet); // We don't want to lose this packet >:D
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerMovement))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.targetPosition);

            SendUDPDataToAll(_packet);
        }
    }

    public static void EnemyTarget(Enemy _enemy, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.changeEnemyTarget))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_player.id);

            SendUDPDataToAll(_packet);
        }
    }

    public static void EnemyTarget(int _toClient, Enemy _enemy, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.changeEnemyTarget))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_player.id);

            SendUDPData(_toClient, _packet);
        }
    }

    public static void RemoveEnemyTarget(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.removeEnemyTarget))
        {
            _packet.Write(_enemy.id);

            SendUDPDataToAll(_packet);
        }
    }

    public static void RemoveEnemyTarget(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.removeEnemyTarget))
        {
            _packet.Write(_enemy.id);

            SendUDPData(_toClient, _packet);
        }
    }

    public static void UpdateEnemyPosition(Enemy _enemy, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateEnemyPosition))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void UpdateEnemyPosition(int _toClient, Enemy _enemy, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updateEnemyPosition))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_position);

            SendUDPData(_toClient, _packet);
        }
    }

    public static void MeleeEnemyAttack(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.meleeEnemyAttack))
        {
            _packet.Write(_enemy.id);

            SendUDPDataToAll(_packet);
        }
    }

    public static void DistanceEnemyAttack(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.distanceEnemyAttack))
        {
            _packet.Write(_enemy.id);

            SendUDPDataToAll(_packet);
        }
    }

    public static void UpdatePlayerHealth(int _toClient, float _health)
    {
        using (Packet _packet = new Packet((int)ServerPackets.updatePlayerHealth))
        {
            _packet.Write(_health);

            SendUDPData(_toClient, _packet);
        }
    }

    public static void PlayerAttackTrigger(int _player, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerAttackTrigger))
        {
            _packet.Write(_player);
            _packet.Write(_rotation);

            SendUDPDataToAll(_player, _packet);
        }
    }

    public static void PlayerAttackEnemy(int _player, int _enemyId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerAttackEnemy))
        {
            _packet.Write(_player);
            _packet.Write(_enemyId);

            SendUDPDataToAll(_player, _packet);
        }
    }

    public static void EnemyHurt(int _enemy, float _health)
    {
        using (Packet _packet = new Packet((int)ServerPackets.enemyHurt))
        {
            _packet.Write(_enemy);
            _packet.Write(_health);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnect(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnect))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet); // We don't want to lose this packet
        }
    }

    public static void PlayerDistanceAttack(int _player, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDistanceAttack))
        {
            _packet.Write(_player);
            _packet.Write(_rotation);

            SendUDPDataToAll(_player, _packet);
        }
    }

    public static void PlayerHurt(int _player, float _damage)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerHurt))
        {
            _packet.Write(_player);
            _packet.Write(_damage);

            SendUDPData(_player, _packet);
        }
    }

    public static void PotionAdded(int _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.potionAdded))
        {
            SendUDPData(_player, _packet);
        }
    }

    public static void SpawnPotion(int _itemId, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPotion))
        {
            _packet.Write(_itemId);

            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnAxe(int _itemId, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnAxe))
        {
            _packet.Write(_itemId);

            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void SpawnMilk(int _itemId, Vector3 _position)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnMilk))
        {
            _packet.Write(_itemId);

            _packet.Write(_position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void DestroyItem(int _itemId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.destroyItem))
        {
            _packet.Write(_itemId);

            SendTCPDataToAll(_packet);
        }
    }

    public static void ConsumePotion(int _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.consumePotion))
        {
            SendUDPData(_player, _packet);
        }
    }

    public static void CorrectInsert(int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.correctInsert))
        {
            SendTCPData(_fromClient, _packet);
        }
    }

    public static void WrongInsert(int _fromClient)
    {
        using (Packet _packet = new Packet((int)ServerPackets.wrongInsert))
        {
            SendTCPData(_fromClient, _packet);
        }
    }
}
