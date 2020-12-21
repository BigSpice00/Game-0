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
    public ActiveWeapon weapon;
    public bool shooting = false;
    public bool stoppedShootingCounter = false;
    public float horozontalRecoil;
    public float verticalRecoil;
    public float horozontalRecoilValue;

    void Start()
    {
        horozontalRecoilValue = 0f;
        verticalRecoil = 0f;
        horozontalRecoil = 0f;
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
            if (shooting)
            {
                xRotation -= verticalRecoil * Time.deltaTime;
                horozontalRecoilValue += Random.Range(-horozontalRecoil, horozontalRecoil);
                stoppedShootingCounter = true;
            }
            

            playerCam.transform.localRotation = Quaternion.Euler(xRotation, horozontalRecoilValue, 0f);
        }
        else
        {
            xRotation -= xRotation;
        }
        

    }
}