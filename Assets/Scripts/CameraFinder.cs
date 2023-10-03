using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// used to set the Main camera to one of the virtual cameras
public class CameraFinder : MonoBehaviour
{
    private CinemachineBrain camBrain;
    public GameObject yuichi;

    // Start is called before the first frame update
    void Start()
    {
        camBrain = this.gameObject.GetComponent<CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camBrain.ActiveVirtualCamera == null)
        {
            CinemachineVirtualCameraBase vCam = GameObject.Find("main").GetComponent<CinemachineVirtualCameraBase>();
            vCam.Priority = 5;
            vCam.Follow = yuichi.transform;
        }
    }
}
