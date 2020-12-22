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
    //public bool stoppedShootingCounter = false;
    public float horozontalRecoil;
    public float verticalRecoil;
    public float horozontalRecoilValue;
    public float recoilDuration = 0f;
    public float time;

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
                time = recoilDuration;
            }
            
            if(time > 0)
            {
                xRotation = xRotation - (((verticalRecoil/1000) * Time.deltaTime) / recoilDuration);
                horozontalRecoilValue += (Random.Range(-horozontalRecoil/10, horozontalRecoil/10f) * Time.deltaTime) / recoilDuration;
                horozontalRecoil = Mathf.Clamp(xRotation, -20f, 20f);
                //stoppedShootingCounter = true;

                time -= Time.deltaTime;
            }

            playerCam.transform.localRotation = Quaternion.Euler(xRotation, horozontalRecoilValue, 0f);
        }
        else
        {
            xRotation -= xRotation;
        }
        

    }
}