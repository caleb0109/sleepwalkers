using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private GameObject gObj;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Press E to collect");
        gObj = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gObj = null;
    }
    void OnInteract()
    {
        gObj.SetActive(false);
    }
}
