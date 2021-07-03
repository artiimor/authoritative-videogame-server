using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonManager : MonoBehaviour
{
    public static bool GameIsPaused=false;
    public GameObject PauseMenuUI;

    public static PauseButtonManager pauseButtonManager;

    void Start()
    {
        /*Instanciacion del singleton*/
        if (pauseButtonManager == null)
        {
            pauseButtonManager = this;
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
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
