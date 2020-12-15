using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public Gun weaponPreFab;

    private void OnTriggerEnter(Collider collider)
    {
        ActiveWeapon activeWeapon = collider.gameObject.GetComponent<ActiveWeapon>();
        if (activeWeapon)
        {
            Gun newWeapon = Instantiate(weaponPreFab);
            activeWeapon.Equip(newWeapon);
        }  
    }
}
