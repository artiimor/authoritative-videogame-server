using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContoller : MonoBehaviour
{
    protected static int maxId = 0;
    public int id;
    // Start is called before the first frame update
    void Awake()
    {
        id = maxId;
        maxId++;
    }
}
