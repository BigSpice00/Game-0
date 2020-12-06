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
    [Space(10)]

    Transform target;
    NavMeshAgent agent;


    void Start()
    {
        target = PlayerManager.instance.Player.transform;
        agent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position);

            if(distance <= agent.stoppingDistance)
            {
                FaceTarget();
                //Attack
            }
        }
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
