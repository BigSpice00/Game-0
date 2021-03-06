﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

public class ActiveWeapon : MonoBehaviour
{
    public enum WeaponSlots
    {
        Primary = 0,
        Secondary = 1
    }
    public Gun[] equipedWeapon = new Gun[2];
    int activeWeaponIndex;
    public Transform[] weaponSlots;
    public MouseLook CameraBoi;
    public Transform LeftGrip;
    public Transform RightGrip;
    public GameObject concreteBulletHole;
    public GameObject woodBulletHole;
    public GameObject sandBulletHole;
    public GameObject metalBulletHole;
    public GameObject softBulletHole;
    public GameObject dirtBulletHole;
    public Camera ShootingCamera;
    public bool isItShooting = false;
    //public Rig handIK;
    public Animator rigController;
    public bool isHolstered = false;
    public bool Silenced = false;
    public float listeningRange = 0f;
    Gun GetWeapon(int index)
    {
        if (index < 0 || index >= equipedWeapon.Length)
        {
            //Debug.Log("Null weapon");
            return null;
        }
        return equipedWeapon[index];
    }
    void Start()
    {
        activeWeaponIndex = -1;
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

        if(activeWeaponIndex != -1)
        {
            if (equipedWeapon[activeWeaponIndex].IsItShooting())
            {
                CameraBoi.verticalRecoil = equipedWeapon[activeWeaponIndex].verticalRecoilValue;
                CameraBoi.horozontalRecoil = equipedWeapon[activeWeaponIndex].horozontalRecoilValue;
                CameraBoi.recoilDuration = equipedWeapon[activeWeaponIndex].recoilDuration;
                CameraBoi.shooting = true;
                listeningRange = equipedWeapon[activeWeaponIndex].range / equipedWeapon[activeWeaponIndex].listeningDropOff;
                Silenced = equipedWeapon[activeWeaponIndex].Silenced;
                isItShooting = true;
            }
            else
            {
                CameraBoi.shooting = false;
                isItShooting = false;
            }        
            if (equipedWeapon[activeWeaponIndex])
            {
                //equipedWeapon[activeWeaponIndex].IsItHolstered(isHolstered);
            }
        }
        


        if (Input.GetKeyDown(KeyCode.X))
        {
            toggleActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveWeapon(WeaponSlots.Primary);
        }        
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveWeapon(WeaponSlots.Secondary);
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ScrollToNextWeapon();
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ScrollToNextWeapon();
        }
    }

    void ScrollToNextWeapon()
    {
        if(activeWeaponIndex == 1 && equipedWeapon[0])
        {
            SetActiveWeapon(WeaponSlots.Primary);
        }
        else if(activeWeaponIndex == 0 && equipedWeapon[1])
        {
            SetActiveWeapon(WeaponSlots.Secondary);
        }
        else
        {
            return;
        }
    }

    public void Equip (Gun newWeapon)
    {
        int weaponSlotIndex = (int) newWeapon.weaponSlots;
        var weapon = GetWeapon(weaponSlotIndex);
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
        weapon.transform.SetParent(weaponSlots[weaponSlotIndex], false);
        Adjust();

        equipedWeapon[weaponSlotIndex] = weapon;
        SetActiveWeapon(newWeapon.weaponSlots);
    }

    public void toggleActiveWeapon()
    {
        bool aimedAndNotPulled = Input.GetMouseButton(1) && isHolstered;
        if (!Input.GetMouseButton(1) || aimedAndNotPulled)
        {        
            //Debug.Log("BOOM2");
            bool Holstered = rigController.GetBool("holstering");
            if (Holstered)
            {
                StartCoroutine(ActivateWeapon(activeWeaponIndex));
            }
            else
            {
                StartCoroutine(HolsterWeapon(activeWeaponIndex));
            }   
        }

    }

    void SetActiveWeapon(WeaponSlots weaponSlot)
    {
        //Debug.Log("1");
        int holsterIndex = activeWeaponIndex;
        int activeIndex = (int)weaponSlot;
        if (holsterIndex == activeIndex)
        {
            return;
        }
        StartCoroutine(SwitchWeapon(activeIndex, holsterIndex));
    } 

    IEnumerator SwitchWeapon (int activeIndex, int holsterIndex)
    {
       // Debug.Log("2");
        activeWeaponIndex = activeIndex;
        yield return StartCoroutine(HolsterWeapon(holsterIndex));
        yield return StartCoroutine(ActivateWeapon(activeIndex));
    }

    IEnumerator HolsterWeapon (int index)
    {
        var weapon = GetWeapon(index);
        if (weapon)
        {
            //Debug.Log("4");
            weapon.Holstered = true;
            rigController.SetBool("holstering", true);
            
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);
        }
    }

    IEnumerator ActivateWeapon (int index)
    {
        var weapon = GetWeapon(index);            
        weapon.Holstered = false;
        if (weapon)
        {
            rigController.SetBool("holstering",false);
            //Debug.Log("3");
            rigController.Play("equip_" + weapon.weaponAnimation);
            do
            {
                yield return new WaitForEndOfFrame();
            } while (rigController.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f);

        }
    }
}
