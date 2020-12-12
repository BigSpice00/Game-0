using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketTurret : MonoBehaviour
{
    public float RateOfFirePerSecond = 0.25f;
    public float WeaponRadius = 150f;
    public float blastRadius = 15f;
    public float blastForce = 950f;
    public float damage = 50f;
    public float rocketSpeed = 50f;
    public float lookingSpeed = 50f;
    public float readyToShootTimer = 0f;
    public GameObject explosionEffect;
    public GameObject Barrel;
    public GameObject rayCastOrigin;
    public GameObject Rocket;
   // public ParticleSystem muzzleFlash;
    GameObject target;
    public GameObject aimTarget;
    GameObject CurrentTarget;
    public float distance;
    RaycastHit hit;
    List<GameObject> targets = new List<GameObject>();
    bool isShooting = false;
    float finalTime;
    float timeToImpact;
    SphereCollider myCollider;
    float currentTime = 0f;
    void Start()
    {
        myCollider = transform.GetComponent<SphereCollider>();
        myCollider.radius = WeaponRadius;
    }

    void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.tag == "Enemy")
        {
            target = collider.gameObject;
            targets.Add(target);
        }
    }
    void Update()
    {
        if (targets.Count != 0)
        {
            if (targets[0] ?? true)
            {
                aimTarget.transform.position = Vector3.Lerp(aimTarget.transform.position, targets[0].transform.position, Time.deltaTime * lookingSpeed);

                if (!isShooting)
                {
                    if(Physics.Raycast(rayCastOrigin.transform.position, -rayCastOrigin.transform.up, out hit, WeaponRadius))
                    {
                        distance = Vector3.Distance(rayCastOrigin.transform.position, hit.point);
                        if (distance <= WeaponRadius)
                        {
                            isShooting = true;
                            finalTime = distance / rocketSpeed;
                            timeToImpact = finalTime;
                        }
                    }

                }
                if (isShooting)
                {
                    if(timeToImpact > 0 && readyToShootTimer <= 0)
                    {
                       // muzzleFlash.Play();
                        currentTime = currentTime + Time.deltaTime;
                        float lerp = currentTime / finalTime;
                        timeToImpact = timeToImpact - Time.deltaTime;
                        Rocket.SetActive(true);
                        if(lerp < 0.25f)
                        {
                            Rocket.transform.position = Vector3.Lerp(Rocket.transform.position, hit.point, lerp);
                        }
                        else
                        {
                            timeToImpact = 0f;
                            Debug.Log("lol");
                        }
                    }
                    else
                    {
                        Debug.Log("pp");
                        Shoot();
                        isShooting = false;
                        Rocket.SetActive(false);
                        Rocket.transform.localPosition = new Vector3(0f, 0f, 0f);
                        currentTime = 0;
                        //distance = Vector3.Distance(rayCastOrigin.transform.position, hit.point);
                    }
                }
                
            }
            else
            {   
                targets.RemoveAt(0);
            }
        }
        if (readyToShootTimer > 0)
        {
            readyToShootTimer = readyToShootTimer - Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (readyToShootTimer <= 0)
        {
            Debug.DrawLine(rayCastOrigin.transform.position, hit.point, Color.red, 1f);
            Instantiate(explosionEffect, hit.point, Quaternion.Euler(transform.up)); 
            Collider[] colliders = Physics.OverlapSphere(hit.point, blastRadius);

            foreach(Collider hitObject in colliders)
            {
                Rigidbody rigidBody = hitObject.GetComponent<Rigidbody>();  
                if(rigidBody != null)
                {
                    rigidBody.AddExplosionForce(blastForce, hit.point, blastRadius);
                }
                if(hitObject.gameObject.tag == "Enemy")
                {
                    EnemyController enemy = hitObject.gameObject.transform.GetComponent<EnemyController>();
                    enemy.TakeDamage(damage);
                }
            }
            readyToShootTimer = 1 / RateOfFirePerSecond;
        }
    }

    }
