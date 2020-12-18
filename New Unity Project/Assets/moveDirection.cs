using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveDirection : MonoBehaviour
{
    public Transform player;
    float pp = 0;
    float pp1 = 0;
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
        pp1 += Time.deltaTime;
        pp = Mathf.Sin(pp1);
        transform.Translate( new Vector3(Vector3.forward.x, pp, Vector3.forward.z) * Time.deltaTime);
    }
}
