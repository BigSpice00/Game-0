using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    [Header("Weapon Stats")]
    public float Damage = 10f;
    public float range = 100f;
    public float RateOfFirePerSecond = 5;
    public bool IsItFullAuto = true;
    [Space(10)]
    [Header("Other")]
    public Camera ShootingCamera;
    public LayerMask IgnoreHuman;
    public float readyToShootTimer = 0f;
    public Recoil recoil;


    void Update ()
    {
        if(readyToShootTimer > 0)
        {
            readyToShootTimer = readyToShootTimer - Time.deltaTime;
        }

            if (IsItFullAuto)
            {
                if (Input.GetButton("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
        }

        

    void Shoot()
    {
        if(readyToShootTimer <= 0) {
            recoil.Fire();
        RaycastHit hit;
        if(Physics.Raycast(ShootingCamera.transform.position, ShootingCamera.transform.forward, out hit, range, ~IgnoreHuman))
        {
            Debug.Log(hit.transform.name);
            EnemyController target = hit.transform.GetComponent<EnemyController>();
            
            if(target != null)
            {
                target.TakeDamage(Damage);
            }
                readyToShootTimer = 1 / RateOfFirePerSecond;
        }
        }
    }

    }





