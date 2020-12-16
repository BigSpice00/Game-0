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
    public float bulletForceOnImpact = 30f;

    [Space(10)]
    [Header("GFX")]
    public ParticleSystem[] muzzleFlash;
    public GameObject concreteBulletHole;
    public GameObject woodBulletHole;
    public GameObject sandBulletHole;
    public GameObject metalBulletHole;
    public GameObject softBulletHole;
    public GameObject dirtBulletHole;
    public GameObject currentBulletHole;
    public float timeTillDeath = 2f;
    public TrailRenderer TracerEffect;
    public GameObject muzzleOrigin;
    public string weaponAnimation;

    [Space(10)]
    [Header("Other")]
    public Camera ShootingCamera;
    public LayerMask IgnoreHuman;
    public float readyToShootTimer = 0f;
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
        if (Input.GetMouseButton(1) && playerControllerScript.IsItGrounded() && playerControllerScript.IsReadyToShoot())
        {

            if (IsItFullAuto)
            {
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                   
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
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
            var tracer = Instantiate(TracerEffect, muzzleOrigin.transform.position, Quaternion.identity);
            tracer.AddPosition(muzzleOrigin.transform.position);
            foreach (ParticleSystem Muzzle in muzzleFlash)
            {
                Muzzle.Emit(1);
            }

            RaycastHit hit;
        if(Physics.Raycast(ShootingCamera.transform.position, ShootingCamera.transform.forward, out hit, range, ~IgnoreHuman))
        {
            Debug.Log(hit.transform.name);
            EnemyController target = hit.transform.GetComponent<EnemyController>();
            
            if(target != null)
            {
                target.TakeDamage(Damage);
            }
            if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * bulletForceOnImpact);
                }
                readyToShootTimer = 1 / RateOfFirePerSecond;
                tracer.transform.position = hit.point;

                if (hit.collider.gameObject.tag == "Dirt")
                {
                    currentBulletHole = dirtBulletHole;
                }
                else if (hit.collider.gameObject.tag == "Sand")
                {
                    currentBulletHole = sandBulletHole;
                }
                else if (hit.collider.gameObject.tag == "Wood")
                {
                    currentBulletHole = woodBulletHole;
                }
                else if (hit.collider.gameObject.tag == "Metal")
                {
                    currentBulletHole = metalBulletHole;
                }
                else if (hit.collider.gameObject.tag == "Concrete")
                {
                    currentBulletHole = concreteBulletHole;
                }
                else
                {
                    currentBulletHole = softBulletHole;
                }                    
                GameObject impactEffect = Instantiate(currentBulletHole, hit.point, Quaternion.Euler(0f, 0f, 0f));
                currentBulletHole.transform.GetChild(0).forward = hit.normal;
                Destroy(impactEffect, timeTillDeath);
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





