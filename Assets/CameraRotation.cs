using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    // Start is called before the first frame update

    //Mouse sensitivity
    public float mouseSensitivity = 100f;

    public Camera cam;

    public Transform boatBody;

    public CinemachineVirtualCamera vCam;

    float xRotation = 0f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //subtrtacting the mouseY from the xRotation or else the camera will flip
        xRotation -= mouseY;

        //Clamping the xRotation to the min and max values so the camera can't fully flip around

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}