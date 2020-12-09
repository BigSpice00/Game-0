using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGoUp : MonoBehaviour
{
    public GameObject[] SpiderLegs;
    public Vector3 SumOfLegs = new Vector3(0f, 0f, 0f);
    void Update()
    {
        foreach (GameObject SpiderLeg in SpiderLegs)
        {
            SumOfLegs = SumOfLegs + SpiderLeg.transform.position;
            Debug.Log(SpiderLeg.transform.position);
        }

        transform.position = new Vector3(transform.position.x / 8f, SumOfLegs.y / 8f + transform.position.y, transform.position.z / 8f);
        SumOfLegs = new Vector3(0f, 0f, 0f);
    }
}
