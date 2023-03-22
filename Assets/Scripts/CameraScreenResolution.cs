using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour
{
    private float deafultWidth;
    public float defaultCameraAspect;

    // Start is called before the first frame update
    void Start()
    {
        //defaultCameraAspect = (float)1920 / (float)1080;
        //Debug.Log(defaultCameraAspect + ", " + Camera.main.aspect);
        deafultWidth = defaultCameraAspect * Camera.main.orthographicSize;

    }

    private void Update()
    {
        ChangeCameraSize();
    }

    public void ChangeCameraSize()
    {
        Camera.main.orthographicSize = deafultWidth / Camera.main.aspect;
    }
}
