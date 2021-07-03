using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;


public class RSA : MonoBehaviour
{
    public static RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(2048);

    public static void addPublicKey(string _XMLPublicKey)
    {
        RSAProvider.FromXmlString(_XMLPublicKey);
    }

    public static byte[] EncriptData(byte[] _data)
    {
        return RSAProvider.Encrypt(_data, false);
    }
}

