using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeaponCaller : MonoBehaviour
{
    #region Singleton

    public static ActiveWeaponCaller instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public ActiveWeapon activeWeapon;
}
