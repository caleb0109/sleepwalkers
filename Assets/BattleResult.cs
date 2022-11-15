using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResult : MonoBehaviour
{
    public bool battleR;
    // Start is called before the first frame update
    void Start()
    {
        battleR = false;
    }

    // Update is called once per frame
    void Update()
    {
        battleR = FindObjectOfType<Scenes>().bResult;
    }
}
