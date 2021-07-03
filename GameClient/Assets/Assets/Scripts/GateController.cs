using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    /*Singleton*/
    public static GateController gateController;

    // Start is called before the first frame update
    void Start()
    {
        /*Instanciacion del singleton*/
        if (gateController == null)
        {
            gateController = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
