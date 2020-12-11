using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextArea : MonoBehaviour
{
    public GameObject MoveCube;
    public GameObject oppisiteLeg;
    public Vector3 OrginalPosition;
    public bool moving = false;
    public float LegMoveSpeed = 8f;

    void Update()
    {
        float distance = Vector3.Distance(MoveCube.transform.position, transform.position);
        if (distance > 0.5f && !oppisiteLeg.GetComponent<MoveToNextArea>().ismoving())
        {
            transform.position = Vector3.Lerp(transform.position, MoveCube.transform.position + new Vector3(0f, -0.2f, 0f), Time.deltaTime * LegMoveSpeed);
            OrginalPosition = transform.position;
            moving = true;

        }
        else
        {
            transform.position = OrginalPosition;
            moving = false;
            
        }


    }
    public bool ismoving()
    {
        return moving;
    }
}
