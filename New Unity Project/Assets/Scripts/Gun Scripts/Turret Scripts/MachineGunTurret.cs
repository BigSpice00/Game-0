using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunTurret : MonoBehaviour
{
    public float RateOfFirePerSecond = 0.25f;
    public float WeaponRadius = 150f;
    public float damage = 50f;
    public float lookingSpeed = 50f;
    public float readyToShootTimer = 0f;
    public GameObject Barrel;
    public GameObject rayCastOrigin;
    GameObject target;
    public GameObject aimTarget;
   // public GameObject impact;
   // public ParticleSystem muzzleFlash;
    GameObject CurrentTarget;
    public float distance;
    RaycastHit hit;
    List<GameObject> targets = new List<GameObject>();
    SphereCollider myCollider;
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
                aimTarget.transform.position = Vector3.Lerp(aimTarget.transform.position, targets[0].transform.position + new Vector3(0f, 0.3f, 0f), Time.deltaTime * lookingSpeed);
                if (Physics.Raycast(rayCastOrigin.transform.position, -rayCastOrigin.transform.up, out hit, WeaponRadius))
                {
                    distance = Vector3.Distance(rayCastOrigin.transform.position, hit.point);
                    if (distance <= WeaponRadius)
                    {
                        Shoot();
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
          //  muzzleFlash.Play();
            //GameObject impactBoi = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactBoi, 2f);
            Debug.DrawLine(rayCastOrigin.transform.position, hit.point, Color.red, 1f);
            EnemyController Enemy = hit.transform.GetComponent<EnemyController>();

            if (Enemy != null)
            {
                Enemy.TakeDamage(damage);
            }
            readyToShootTimer = 1 / RateOfFirePerSecond;
        }
    }

}