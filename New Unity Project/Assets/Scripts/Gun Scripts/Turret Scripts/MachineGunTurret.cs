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
            foreach (ParticleSystem Muzzle in muzzleFlash)
            {
                Muzzle.Emit(1);
            }

            Debug.DrawLine(rayCastOrigin.transform.position, hit.point, Color.red, 1f);
            EnemyController Enemy = hit.transform.GetComponent<EnemyController>();

            if (Enemy != null)
            {
                Enemy.TakeDamage(damage);
            }
            readyToShootTimer = 1 / RateOfFirePerSecond;

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