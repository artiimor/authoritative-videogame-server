using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadController : MonoBehaviour
{
    public PlayerCombat Kevin;
    public GameObject deathScreen;
    public static DeadController deadController;
    // Update is called once per frame

    void Start()
    {
        /*Instanciacion del singleton*/
        if (deadController == null)
        {
            deadController = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        /*if (Kevin.health <= 0)
        {
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
        }*/
    }
}
