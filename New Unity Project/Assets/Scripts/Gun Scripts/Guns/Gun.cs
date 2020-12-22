using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Weapon Stats")]
    public float Damage = 10f;
    public float range = 100f;
    public float RateOfFirePerSecond = 5f;
    public bool IsItFullAuto = true;
    public float listeningDropOff = 2f;
    public bool Silenced = false;
    public float bulletForceOnImpact = 30f;

    [Space(10)]
    [Header("Recoil")]
    public float verticalRecoilValue = 25f;
    public float horozontalRecoilValue = 0.5f;
    public float recoilDuration = 0.5f;
    //public MouseLook CameraBoi;
    public Cinemachine.CinemachineImpulseSource cameraShake;

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
    public ActiveWeapon.WeaponSlots weaponSlots;
    public Camera ShootingCamera;
    public LayerMask IgnoreHuman;
    public float readyToShootTimer = 0f;
    PlayerController playerControllerScript;
    public bool Shooting = false;
    public bool Holstered = true;
    
    void Start()
    {
        cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
        playerControllerScript = PlayerController.instance;
    }
    
    void Update ()
    {
        //CameraBoi.shootingp(Shooting);
        if (readyToShootTimer > 0)
        {
            readyToShootTimer = readyToShootTimer - Time.deltaTime;
            Shooting = false;
        }
        if (Input.GetMouseButton(1) && playerControllerScript.IsItGrounded() && playerControllerScript.IsReadyToShoot() && !Holstered && readyToShootTimer <= 0)
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
        }
        else
        {
            Shooting = false;
        }

        }

        

    void Shoot()
    {
        if(readyToShootTimer <= 0) {
            readyToShootTimer = 1 / RateOfFirePerSecond;
            Shooting = true;
            var tracer = Instantiate(TracerEffect, muzzleOrigin.transform.position, Quaternion.identity);
            cameraShake.GenerateImpulse(ShootingCamera.transform.forward);
            tracer.AddPosition(muzzleOrigin.transform.position);
            foreach (ParticleSystem Muzzle in muzzleFlash)
            {
                Muzzle.Emit(1);
            }
            //AimFollow.transform.rotation = Quaternion.Euler(new Vector3(AimFollow.transform.rotation.x + 100f, AimFollow.transform.rotation.y, AimFollow.transform.rotation.z));

            RaycastHit hit;
        if(Physics.Raycast(ShootingCamera.transform.position, ShootingCamera.transform.forward, out hit, range, ~IgnoreHuman))
        {
            //Debug.Log(hit.transform.name);
            EnemyController target = hit.transform.GetComponent<EnemyController>();
            
            if(target != null)
            {
                target.TakeDamage(Damage);
                target.attacked(range);
            }
            if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * bulletForceOnImpact);
                }
                
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
    public void IsItHolstered(bool holstered)
    {
        Holstered = holstered;
    }

    }