using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CameraController : MonoBehaviour
{
    public Transform playerPosition;
    public Vector3 offset;
    public Vector3 quaternionOffset;

    /*Singleton*/
    public static CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        /*Instanciacion del singleton*/
        if (cameraController == null)
        {
            /*GameObject temp = GameObject.Find("Kevin");
            Transform kevinTransform = temp.GetComponent<Transform>();
            playerPosition = kevinTransform;*/

            cameraController = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerPosition != null)
            transform.position = playerPosition.position + offset;
        // transform.rotation = new Quaternion(quaternionOffset.x, quaternionOffset.y, quaternionOffset.z, 0f);
    }
}
