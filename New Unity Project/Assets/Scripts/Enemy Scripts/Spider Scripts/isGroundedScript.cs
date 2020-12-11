using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGroundedScript : MonoBehaviour
{
    public GameObject Object2;
    int layerMask;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Ground");
    }
    void Update()
    {   
        RaycastHit hit;
        if(Physics.Raycast(Object2.transform.position + new Vector3(0f, 1.5f, 0f), -transform.up, out hit, layerMask))
        {
            transform.position = hit.point + new Vector3(0f, 0.18f, 0f);
        }
    }
}
