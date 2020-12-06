using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotateboi : MonoBehaviour
{
    public float LookingSpeed = 5f;
    public Transform target;

    public void Update()
    {
    Vector3 direction = (target.position - transform.position).normalized;
    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime* LookingSpeed);
}
}

