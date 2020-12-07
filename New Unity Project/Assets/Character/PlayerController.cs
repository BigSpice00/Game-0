/*--Bugs--*
 
- There is a bug with movement while aiming down sight making movement inverted 

--= to Do =-- 

-Create animations and speeds for mr brute
-Add sprint and crouch

*/
using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    #region Singleton

    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }

    #endregion
    // ^ ^ i made a singleton to make it easier to reference this script from other scripts cuz this one is important ^ ^

    [Header("Stats")]
    public float MaxHealth = 100f;
    public float Health;
    public float HealthRegenSpeed = 3f;
    public float TimeToHealthRegen = 3f;
    public float TimeToRecoverRemaining = 0f;
    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;
    Vector3 velocity;
    bool isGrounded;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    [Space(10)]
    [Header("Controller and camera")]
    public CharacterController controller;
    public Transform cam;

    [Space(10)]
    [Header("ground checker for jumpping")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Space(10)]
    [Header("Camera for aimming in third person")]
    public GameObject FollowCamera;
    public GameObject AimCamera;

   

    void Start()
    {
        Health = MaxHealth;
    }

    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //movement and wlaking 

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (Input.GetMouseButton(1)) // to walk without spinning while aiming
            {
                controller.Move(direction.normalized * speed * Time.deltaTime);
            }
            else //to spin towards where camera is pointing and walk without aiming
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

        }

        if (Input.GetMouseButtonDown(1) && isGrounded) //to make the boi aim at where the camera is pointing and not where he is pointing
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }


        if (Input.GetMouseButton(1) && isGrounded) //some code for the camera system for aiming and shit no?
        {
            if (!AimCamera.activeInHierarchy)
            {
                AimCamera.SetActive(true);
                FollowCamera.SetActive(false);
            }

        }
        else
        {
            AimCamera.SetActive(false);
            FollowCamera.SetActive(true);
        }

        if (TimeToRecoverRemaining > 0)  //to recover health after taking damage
        {
            TimeToRecoverRemaining = TimeToRecoverRemaining - Time.deltaTime;
        }
        else
        {
            TimeToRecoverRemaining = 0;
            if(Health < MaxHealth)
            {
                Health = Health + (Time.deltaTime * HealthRegenSpeed);
            }
            else
            {
                Health = MaxHealth;
            }
        }

        if(Health <= 0)
        {
            // insert die code here pls
        }
    }
        
    public bool IsItGrounded() //to send to other scripts that the boi is grounded
    {
        return isGrounded;
    }

    public void TakeDamage(float DamageTaken) //to be attacked by enemies
    {
        Health = Health - DamageTaken;
        TimeToRecoverRemaining = TimeToHealthRegen;
    }
}