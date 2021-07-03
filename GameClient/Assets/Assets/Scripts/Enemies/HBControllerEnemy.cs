using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HBControllerEnemy : MonoBehaviour
{
    public Slider mainSlider;
    public GameObject hbUI;

    public EnemyStats enemy;

    // public static HealthbarController healthbarController;

    public void Start()
    {
        /*Vida maxima*/
        mainSlider.maxValue = 100;

        /*Vida real*/
        float health = 100f;
        mainSlider.value = health;
    }

    public void FixedUpdate()
    {
        SetHealth(enemy.getHealth());
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
