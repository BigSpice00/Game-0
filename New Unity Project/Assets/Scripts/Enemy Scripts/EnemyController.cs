/*
 Try the noise attraction again
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    
    [Header("Enemy Stats")]
    public float health = 100f;
    public float Resistance = 15f;
    public float LookRadius = 10f;
    public float LookingSpeed = 5f;
    public float Damage = 5f;
    public float TimeBetweenAttacks = 5f;
    public float TimeBetweenAttacksLeft = 0f;
    [Space(10)]

    Transform target;
    NavMeshAgent agent;
    PlayerController playerControllerScript;
    //Gun gun;
    public float lookRadiusTemp;
    //float timeAfterShot = 0;

    void Start()
    {
        lookRadiusTemp = LookRadius;
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
        playerControllerScript = PlayerController.instance;
        //gun = Gun.instance;
    }


    void Update()
    {

        if (TimeBetweenAttacksLeft <= 0)
        {
            TimeBetweenAttacksLeft = 0;
        }
        else
        {
            TimeBetweenAttacksLeft = TimeBetweenAttacksLeft - Time.deltaTime;
        }


            float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                FaceTarget();

                if(TimeBetweenAttacksLeft <= 0) 
                {
                    playerControllerScript.TakeDamage(Damage);
                    TimeBetweenAttacksLeft = TimeBetweenAttacks;
                }


            }
        }
        //Attempt at noise attraction from gun
        /*if(timeAfterShot < 0)
        {
            LookRadius = lookRadiusTemp;
            timeAfterShot = 0;
        }
        else
        {
            timeAfterShot = timeAfterShot - Time.deltaTime;
        }
        if (gun.IsItShooting() && gun.IsItNotSilenced() && gun.WhatsListeningRange() <= distance)
        {
            LookRadius = gun.WhatsListeningRange();
            timeAfterShot = 1f;
        } */
    }

    public void TakeDamage (float Damage)
    {
        health -= (Damage * (1-(Resistance / 100)));

        if(health <= 0f)
        {
            Die();
        }
    }

     void Die()
    {
        Destroy(gameObject);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * LookingSpeed);
    }

    void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
