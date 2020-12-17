/*--Bugs--*
 
when aiming down the game remembers the last rotation of the camera and applies it when aiming again (aim down sight look all the way up and then 
let go of the aim then try to aim again but looking all the way down u will know)
--= to Do =-- 

- insert die code here pls

*/
using System.Collections;


using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    public float SprintSpeed = 12;
    public float crouchSpeed = 4;
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
    public GameObject FreeLookObject;

    [Space(10)]
    [Header("Animation Rigging")]
    public Rig aimLayer;
    public float toAimDuration = 0.3f;
    public Animator rigController;
    public GameObject weaponAimToCenter;
    public ActiveWeapon activeBoi;

    Animator animator;
    float CurrentSpeed = 0f;
    float walkingAcceleration = 1f;
    float SprintAcceleration = 2f;
    float walkingAccelerationTemp;
    float speedTemp;
    bool IsSprinting = false;
    Vector2 input;




    void Start()
    {
        activeBoi = GetComponent<ActiveWeapon>();
        walkingAccelerationTemp = walkingAcceleration;
        speedTemp = speed;
        Health = MaxHealth;
        animator = GetComponent<Animator>();
        animator.SetBool("Crouching", false);
        animator.SetBool("Aiming", false);
    }

    void Update()
    {
        animator.SetFloat("Speed", CurrentSpeed); //set the animation speed
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //movement and wlaking 

        if (Input.GetKey(KeyCode.LeftControl) && isGrounded) //crouching
        {
            speed = crouchSpeed;
            walkingAcceleration = walkingAccelerationTemp;
            IsSprinting = false;
            animator.SetBool("Crouching", true);
            FreeLookObject.transform.localPosition = new Vector3(FreeLookObject.transform.localPosition.x, 0.9f, FreeLookObject.transform.localPosition.z);//to move the camera to the crouching animation
        }
        else
        {
            animator.SetBool("Crouching", false);
            FreeLookObject.transform.localPosition = new Vector3(FreeLookObject.transform.localPosition.x, 1.6f, FreeLookObject.transform.localPosition.z);

        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }
        if (isGrounded)
        {
            animator.SetBool("InAir", false);
            if (Input.GetButton("Jump"))
            {
                animator.SetBool("Jumping", true);
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            else
            {
                animator.SetBool("Jumping", false);
            }
        }
        else
        {
            animator.SetBool("InAir", true);
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.applyRootMotion = false;
            if (Input.GetKey(KeyCode.LeftControl) && isGrounded ) // to move the camera up a bit while crouch moving
            {
                FreeLookObject.transform.position = new Vector3(FreeLookObject.transform.position.x, 1.2f, FreeLookObject.transform.position.z);
            }
            if (!Input.GetMouseButton(1)) { 
                if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
                {
                    speed = SprintSpeed;
                    walkingAcceleration = SprintAcceleration;
                    IsSprinting = true;
                }
                else if(!Input.GetKey(KeyCode.LeftControl))
                {
                    speed = speedTemp;
                    walkingAcceleration = walkingAccelerationTemp;
                    IsSprinting = false;
                }

                


                addSpeed(true, false, IsSprinting);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * speed * Time.deltaTime);
            }

        }
        else
        {
            addSpeed(false, false, false);
        }


        if (Input.GetMouseButtonDown(1) && isGrounded) //to make the boi aim at where the camera is pointing and not where he is pointing
        {
            //if (!rigController.GetBool("holstering"))
           //{
               // Debug.Log("BOOM");
                //activeBoi.toggleActiveWeapon();
            //}
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            FreeLookObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if (Input.GetMouseButton(1) && isGrounded) //some code for the camera system for aiming and shit no?
        {
            animator.applyRootMotion = true;
            animator.SetBool("Aiming", true);
            rigController.SetBool("Aiming", true);
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
            animator.SetFloat("MovementX", input.x);
            animator.SetFloat("MovementY", input.y);
            if (!AimCamera.activeInHierarchy)
            {
            AimCamera.SetActive(true);
            FollowCamera.SetActive(false);
            }

        }
        else
        {
            weaponAimToCenter.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            animator.applyRootMotion = false;
            AimCamera.SetActive(false);
            FollowCamera.SetActive(true);
            animator.SetBool("Aiming", false);
            rigController.SetBool("Aiming", false);
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
    
    public bool IsReadyToShoot() //to send to other scripts that the boi is grounded
    {
        if (aimLayer.weight >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(float DamageTaken) //to be attacked by enemies
    {
        Health = Health - DamageTaken;
        TimeToRecoverRemaining = TimeToHealthRegen;
    }
    
    public void addSpeed(bool isGoing, bool IsAiming, bool isSprinting) // a code that sets the animation for speed (ik way more complecated that it should be)
    {
        if (isGoing && !IsAiming && isSprinting && CurrentSpeed <= 1f)
        {
            CurrentSpeed = CurrentSpeed + (Time.deltaTime * walkingAcceleration);
        }
        else if (isGoing && !isSprinting && CurrentSpeed <= 0.5f)
        {
            CurrentSpeed = CurrentSpeed + (Time.deltaTime * walkingAcceleration);
        }
        else if (isGoing && !isSprinting && CurrentSpeed > 0.5f)
        {
            CurrentSpeed = CurrentSpeed - (Time.deltaTime * walkingAcceleration);
        }
        else if (!isGoing && CurrentSpeed > 0f)
        {
            animator.applyRootMotion = true;
            CurrentSpeed = CurrentSpeed - (Time.deltaTime * walkingAcceleration)*2;
        }
        else if (!isGoing && CurrentSpeed < 0f)
        {
            animator.applyRootMotion = false;
            CurrentSpeed = 0f;
        }
    }
}