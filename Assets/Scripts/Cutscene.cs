using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Programmer: Jessica Niem
/// Date: 8/6/23
/// Description: Takes care of the movements in the cutscene
/// </summary>
public class Cutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // turns the sprite to whatever direction is inputed
    public void FaceDirection(string direction)
    {
        switch (direction)
        {
            case "forward":
                break;

            case "left":
                break;

            case "backward":
                break;

            case "right":
                break;
        }
    }

    public void MoveRight(float dist)
    {

    }

    public void MoveLeft(float dist)
    {

    }

    public void MoveForward(float dist)
    {

    }

    public void MoveBacward(float dist)
    {

    }

    // used for yuichi if he's not at the correct starting position for the animation
    public void MoveToStartingPos(Vector2 currentPos, Vector2 startingPos)
    {
        Debug.Log("Current: " + currentPos + " Start: " + startingPos);

        if (currentPos != startingPos)
        {
            
        }
    }
}
