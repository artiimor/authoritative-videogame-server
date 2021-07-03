using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void Login()
    {
        // Debug.Log("En login: " + RSA.RSAProvider.ToXmlString(false));
        using (Packet _packet = new Packet((int)ClientPackets.login))
        {
            // Debug.Log("[Login] Us: " + UIManager.instance.usernameField.text + " Password: " + UIManager.instance.passwordField.text);
            _packet.Write(UIManager.instance.usernameField.text);
            _packet.Write(UIManager.instance.passwordField.text);

            _packet.WriteAtBegining((int)ClientPackets.login);

            // _packet.DebugPacket();
            _packet.Cypher();

            _packet.WriteAtBegining((int)ClientPackets.RSAPackage); // To identify RSA cyphered message

            SendTCPData(_packet);
        }
    }

    public static void SignUp()
    {
        // Debug.Log("En login: " + RSA.RSAProvider.ToXmlString(false));
        using (Packet _packet = new Packet((int)ClientPackets.signup))
        {
            // Debug.Log("[Login] Us: " + UIManager.instance.usernameField.text + " Password: " + UIManager.instance.passwordField.text);
            _packet.Write(UIManager.instance.userSignUp.text);
            _packet.Write(UIManager.instance.emailSignUp.text);
            _packet.Write(UIManager.instance.passwordSignUp.text);

            _packet.WriteAtBegining((int)ClientPackets.signup);

            // _packet.DebugPacket();
            _packet.Cypher();

            _packet.WriteAtBegining((int)ClientPackets.RSAPackage); // To identify RSA cyphered message

            SendTCPData(_packet);
        }
    }

    /*NOTE: This one is cyphered using RSA*/
    public static void LoginReceived()
    {
        // Debug.Log("En login received: " + RSA.RSAProvider.ToXmlString(false));
        using (Packet _packet = new Packet((int)ClientPackets.loginReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(Client.instance.playerId);
            _packet.Write(Client.instance.username);

            // _packet.WriteAtBegining((int)ClientPackets.loginReceived);

            // _packet.DebugPacket();
            // _packet.Cypher();

            // _packet.WriteAtBegining((int)ClientPackets.RSAPackage); // To identify RSA cyphered message

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _newTarget)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_newTarget);

            // Debug.Log("FINAL DEL ENVIO DE LA POS :))");
            SendUDPData(_packet);
        }
    }

    public static void InGame()
    {
        using (Packet _packet = new Packet((int)ClientPackets.inGame))
        {
            SendTCPData(_packet); /*We don't want to lose this packet*/
        }
    }

    public static void PlayerAttack(Vector3 _attackPosition, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerAttack))
        {
            _packet.Write(_attackPosition);
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerAttackEnemy(int _enemyId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerAttackEnemy))
        {
            _packet.Write(_enemyId);

            SendUDPData(_packet);
        }
    }

    public static void PlayerDistanceAttack(Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerDistanceAttack))
        {
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerRestoreHealth()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerRestoreHealth))
        {
            // Debug.Log("Enviando paquete para curarnos");
            SendUDPData(_packet);
        }
    }

    #endregion
}
