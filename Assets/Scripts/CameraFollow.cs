 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform pTransform;


    void Awake()
    {
        Debug.Log("I'm up, I'm up!");
        pTransform = GameObject.FindGameObjectWithTag("Player").transform;

        this.gameObject.transform.position = pTransform.position;
    }

    // Update is called once per frame
    /*void LateUpdate()
    {
        //current camera position
        Vector3 temp = transform.position;

        //camera positon is set to player position
        temp.x = pTransform.position.x;
        temp.y = pTransform.position.y;

        transform.position = temp;
    }*/
}
