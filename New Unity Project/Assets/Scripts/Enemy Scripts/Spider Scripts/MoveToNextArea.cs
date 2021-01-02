using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextArea : MonoBehaviour
{
    public GameObject MoveCube;
    public GameObject oppisiteLeg;
    public Vector3 OrginalPosition;
    public bool moving = false;
    public float LegMoveSpeed = 10f;

    void Update()
    {
        float distance = Vector3.Distance(MoveCube.transform.position, transform.position);
        if (distance > 0.56f && !oppisiteLeg.GetComponent<MoveToNextArea>().ismoving())
        {
            transform.position = Vector3.Lerp(transform.position, MoveCube.transform.position + new Vector3(0f, 0.2f, 0f), Time.deltaTime * LegMoveSpeed);
            OrginalPosition = transform.position;
            moving = true;

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(OrginalPosition.x, MoveCube.transform.position.y - 0.5f, OrginalPosition.z), Time.deltaTime * 30f);
            moving = false;
        }


    }
    public bool ismoving()
    {
        return moving;
    }
}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToNextArea : MonoBehaviour
{
    public GameObject MoveCube;
    public GameObject oppisiteLeg;
    public Vector3 OrginalPosition;
    public bool moving = false;
    public float LegMoveSpeed = 10f;
    float HightToGo = 0f;
    float TimeHolder = 0f;
    float HightCap = 1f;

    void Update()
    {
        float distance = Vector3.Distance(MoveCube.transform.position, transform.position);
        if (distance > 0.4f && !oppisiteLeg.GetComponent<MoveToNextArea>().ismoving())
        {
            Vector3 direction = (MoveCube.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f);
            TimeHolder += Time.deltaTime;
            HightToGo = Mathf.Sin(TimeHolder);
            HightToGo = HightToGo / HightCap;
            transform.Translate(new Vector3(Vector3.forward.x, HightToGo + MoveCube.transform.position.y + 0.2f, Vector3.forward.z) * Time.deltaTime);
            moving = true;
            OrginalPosition = transform.position;
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
}*/
