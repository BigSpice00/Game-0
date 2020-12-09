using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderGoUp : MonoBehaviour
{
    public GameObject[] LeftSpiderLegs;

    public GameObject[] RightSpiderLegs;
    public Vector3[] numbers;
    public Vector3 total;
    void Start()
    {
        for (int i = 0; i <= 3; i++)
        {
            numbers[i] = RightSpiderLegs[i].transform.position - LeftSpiderLegs[i].transform.position;
            total = numbers[i];
        }
        total = new Vector3(total.x / 4f, total.y / 4f, total.z / 4f);
        transform.rotation = Quaternion.Euler(total.x, transform.rotation.y, transform.rotation.z);
    }
    void Update()
    {

    }
}
