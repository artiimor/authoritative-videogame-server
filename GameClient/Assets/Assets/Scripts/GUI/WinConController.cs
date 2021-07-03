using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConController : MonoBehaviour
{
    public BossController boss;
    public GameObject winScreen;
    public static WinConController winCon;

    void Start()
    {
        /*Instanciacion del singleton*/
        if (winCon == null)
        {
            winCon = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (boss.dead)
        {
            Time.timeScale = 0f;
            winScreen.SetActive(true);
        }
    }
}
