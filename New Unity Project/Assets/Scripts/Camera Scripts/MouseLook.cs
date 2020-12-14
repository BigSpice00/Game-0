using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSpeed = 100f;
    public Transform playerBody;
    public Transform playerCam;
    float xRotation = 0f;
    public GameObject AimCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (AimCamera.activeInHierarchy)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;


            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            playerBody.Rotate(Vector3.up * mouseX);
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        

    }
}