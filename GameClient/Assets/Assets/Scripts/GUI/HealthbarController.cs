using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{

    public Slider mainSlider;
    public GameObject hbUI;

    public PlayerCombat playerCombat;

    // public static HealthbarController healthbarController;

    public void Start()
    {
        /*Vida maxima*/
        mainSlider.maxValue = 100;

        /*Vida real*/
        // GameObject temp = GameObject.Find("Kevin");
        // float health = temp.GetComponent<PlayerCombat>().getHealth();
        float health = 100f;
        mainSlider.value = health;

        // healthbarController = this;
        // DontDestroyOnLoad(gameObject);
        // DontDestroyOnLoad(transform.parent.gameObject);

        /*else
        {
            if (transform.parent.parent == null)
            {
                Destroy(transform.parent.gameObject);
            }
            
        }*/
        playerCombat = GameObject.Find("LocalPlayer(Clone)").GetComponentInChildren<PlayerCombat>();
    }

    public void FixedUpdate()
    {
        if (playerCombat == null)
        {
            playerCombat = GameObject.Find("LocalPlayer(Clone)").GetComponentInChildren<PlayerCombat>();
        }
        SetHealth(playerCombat.getHealth());
    }

    public void SetMaxHealth(float max)
    {
        mainSlider.maxValue = max;
        mainSlider.value = max;
    }

    public void SetHealth(float health)
    {
        mainSlider.value = health;
    }
}
