using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grounded : MonoBehaviour
{
    int layerMask;
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), -transform.up, out hit, layerMask))
        {
            transform.position = hit.point + new Vector3(0f, 0.2f, 0f);
        }
    }
}
