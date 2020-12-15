﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    Gun weapon;
    public Transform weaponParent;
    public Transform LeftGrip;
    public Transform RightGrip;
    public GameObject concreteBulletHole;
    public GameObject woodBulletHole;
    public GameObject sandBulletHole;
    public GameObject metalBulletHole;
    public GameObject softBulletHole;
    public GameObject dirtBulletHole;
    public Camera ShootingCamera;
    //public Rig handIK;
    public Animator rigController;
    
    void Start()
    {
        Invoke(nameof(Adjust), 0.01f);
        Gun existingWeapon = GetComponentInChildren<Gun>();
        if (existingWeapon)
        {
            Equip(existingWeapon);
        }
    }

    void Adjust()
    {
        rigController.updateMode = AnimatorUpdateMode.AnimatePhysics;
        rigController.cullingMode = AnimatorCullingMode.CullUpdateTransforms;
        Invoke(nameof(Adjust2), 0.05f);

    }        
    void Adjust2()
    {
        rigController.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        rigController.updateMode = AnimatorUpdateMode.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            bool isHolstered = rigController.GetBool("holstering");
            rigController.SetBool("holstering", !isHolstered);
        }
    }

    public void Equip (Gun newWeapon)
    {
        if (weapon)
        {
            Destroy(weapon.gameObject);
        }
        weapon = newWeapon;
        weapon.concreteBulletHole = concreteBulletHole;
        weapon.woodBulletHole = woodBulletHole;
        weapon.sandBulletHole = sandBulletHole;
        weapon.metalBulletHole = metalBulletHole;
        weapon.softBulletHole = softBulletHole;
        weapon.dirtBulletHole = dirtBulletHole;
        weapon.ShootingCamera = ShootingCamera;
        weapon.transform.parent = weaponParent;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        Adjust();
        Invoke(nameof(PlayAnimation), 0.05f);

        
    }

    void PlayAnimation()
    {
        rigController.Play("equip_" + weapon.weaponAnimation);
    }
}
