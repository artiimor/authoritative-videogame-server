using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class RSA : MonoBehaviour
{
    public static RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(2048);

    public static string getPublicKey()
    {
        return RSAProvider.ToXmlString(false);
    }

    public static byte[] DecryptData(byte[] _data)
    {
        return RSAProvider.Decrypt(_data, false);
    }
}
