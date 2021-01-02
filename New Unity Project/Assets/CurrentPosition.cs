using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPosition : MonoBehaviour
{
    Vector3 originalPosition;
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = originalPosition;
    }
}
