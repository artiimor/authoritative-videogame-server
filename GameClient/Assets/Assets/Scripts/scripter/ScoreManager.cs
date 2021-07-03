using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    private int distance = 15;
    private int melee = 15;

    public Text distanceText;
    public Text meleeText;

    /*Singleton*/
    public static ScoreManager scoreManager;

    void Start()
    {
        /*Instanciacion del singleton*/
        if (scoreManager == null)
        {            
            // distanceText = GameObject.Find("DistanceEnemyText").GetComponent<Text>();
            // distanceText.text = "You must kill " + distance + " distance enemies";
            // meleeText.text = "You must kill " + melee + " melee enemies";


            scoreManager = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            updateData();
            Destroy(gameObject);
        }
        
    }

    void update()
    {
        if (distanceText == null)
        {
            distanceText = GameObject.Find("DistanceEnemyText").GetComponent<Text>();
        }
        if (meleeText == null)
        {
            meleeText = GameObject.Find("MeleeEnemyText").GetComponent<Text>();
        }
        updateData();
    }

    public void distanceKilled()
    {
        if (distance > 0)
            distance--;

        distanceText.text = "You must kill " + distance + " distance enemies";

        if (checkObjective())
        {
            openDoor();
        }
    }

    public void meleeKilled()
    {
        if (melee > 0)
            melee--;

        meleeText.text = "You must kill " + melee + " melee enemies";

        if (checkObjective())
        {
            openDoor();
        }
    }

    public int getMelee()
    {
        return melee;
    }

    public int getDistance()
    {
        return distance;
    }

    public string getMeleeString()
    {
        return melee + "";
    }

    public string getDistanceString()
    {
        return distance + "";
    }

    public void updateData()
    {
        if (melee > 0)
            meleeText.text = "You must kill " + melee + " melee enemies";
        else
            meleeText.text = " ";

        if (distance > 0)
            distanceText.text = "You must kill " + distance + " distance enemies";
        else
            distanceText.text = " ";

        if (checkObjective())
            openDoor();
    }

    public bool checkObjective()
    {
        // Debug.Log((melee <= 0 && distance <= 0) + "AASDAS");
        return (melee <= 0 && distance <= 0);
    }

    public void openDoor()
    {
        GameObject temp = GameObject.Find("Gate");
        Animator anim = temp.GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            anim.SetTrigger("OpenFence");
        }
    }

}
