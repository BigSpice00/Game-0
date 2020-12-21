using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    public GameObject AimFollow;
    public void generateRecoil()
    {
        AimFollow.transform.rotation = Quaternion.Euler(new Vector3(AimFollow.transform.rotation.x + 1f, AimFollow.transform.rotation.y, AimFollow.transform.rotation.z));
    }

    public void setAimFollow(GameObject aimBoi)
    {
        AimFollow = aimBoi; 
    }

        void Update()
    {
        
    }
}
