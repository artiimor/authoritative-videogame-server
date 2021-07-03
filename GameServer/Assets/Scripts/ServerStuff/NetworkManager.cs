using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public GameObject playerPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else if (Instance != this)
        {
            Debug.Log("Instance already exist. Removing object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        // #if UNITY_EDITOR
        Debug.Log("You should be running it when you already built it :D");
        // #else
        Server.Start(50, 26950);
        // #endif
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }
}
