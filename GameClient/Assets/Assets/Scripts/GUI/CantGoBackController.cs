using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CantGoBackController : MonoBehaviour
{
    public static CantGoBackController cantController;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        /*Instanciacion del singleton*/
        if (cantController == null)
        {
            cantController = this;
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
        Scene scene = SceneManager.GetActiveScene();
        
        if (scene.name == "Boss")
        {
            button.interactable = false;
        } 
    }
}
