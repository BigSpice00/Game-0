using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    #region Singleton

    public static Gun instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    // ^ ^ i made a singleton to make it easier to reference this script from other scripts cuz this one is important ^ ^

    [Header("Weapon Stats")]
    public float Damage = 10f;
    public float range = 100f;
    public float RateOfFirePerSecond = 5f;
    public bool IsItFullAuto = true;
    public float listeningDropOff = 2f;
    public bool Silenced = false;
    [Space(10)]
    [Header("Other")]
    public Camera ShootingCamera;
    public LayerMask IgnoreHuman;
    public float readyToShootTimer = 0f;
    public Recoil recoil;
    PlayerController playerControllerScript;
    public bool Shooting = false;




    void Start()
    {
        playerControllerScript = PlayerController.instance;
    }

    void Update ()
    {
        
        if (readyToShootTimer > 0)
        {
            readyToShootTimer = readyToShootTimer - Time.deltaTime;
        }
        if (Input.GetMouseButton(1) && playerControllerScript.IsItGrounded())
        {

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
            Shooting = true;
        }
        else
        {
            Shooting = false;
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

    public bool IsItNotSilenced () 
    {
        return !Silenced;
    }
    public bool IsItShooting()
    {
        return Shooting;
    }
    public float WhatsListeningRange()
    {
        return range/listeningDropOff;
    }

    }





