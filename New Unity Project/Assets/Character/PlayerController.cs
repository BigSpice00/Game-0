using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 3;
    public float braceDistance = 0.3f;
    public float airControlPercent = 0.6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float groundRotateSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;
    Vector3 yRotation;
    Vector3 groundRotation;
    Vector3 groundNormal;

    //public float slopeForce = 10;
    //public float slopeForceRayLength = 1.5f;

    Animator animator;
    Transform cameraT;
    CharacterController controller;
    public AudioSource punchSound;

    /* 
     * ---------------BUG NOTES---------------
     * - Velocity remains at negative value for one frame after landing.
     *   Currently causes no issues.
     */

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //INPUT
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        
        Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out RaycastHit hitInfo, Mathf.Infinity);
        groundNormal = hitInfo.normal;

        //Rotate();
        Move(inputDir, running);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //ANIMATION
        HandleAnimations2();
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

    }

    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            yRotation = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
            //groundRotation = new Vector3(Quaternion.LookRotation(transform.forward, groundNormal).eulerAngles.x, 0, Quaternion.LookRotation(transform.forward, groundNormal).eulerAngles.z);
            //transform.eulerAngles = yRotation + groundRotation;
            transform.eulerAngles = yRotation;
        }
        
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        
        Debug.DrawRay(transform.position + Vector3.up, velocity, Color.red);
        Debug.DrawRay(transform.position + Vector3.up, Vector3.Cross(transform.forward, Vector3.right), Color.green); //gravityDir

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = gravity;
            animator.SetBool("kickBool", false);
        }
        else
        {
            velocityY += Time.deltaTime * gravity;
        }
    }

    void Rotate()
    {
        controller.transform.rotation = Quaternion.LookRotation(transform.forward, groundNormal);
    }

    void HandleAnimations()
    {
        animator.SetBool("groundBool", controller.isGrounded);

        RaycastHit hitInfo;

        if (velocityY < 0 && !controller.isGrounded)
        {
            Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity);
            
            if (hitInfo.distance > braceDistance)
            {
                animator.CrossFade("Falling", 0.5f);
            }
            else if (hitInfo.distance <= braceDistance && hitInfo.distance > 0.14f)
            {
                animator.CrossFade("Bracing", 0.1f, 0, 0.4f);
            }
            else  if (controller.isGrounded && hitInfo.distance <= 0.14f) //fixes floating
            {
                animator.CrossFade("Blend Tree", 0.1f);
            }
            Debug.Log(hitInfo.distance);
        }
    }

    void HandleAnimations2()
    {
        animator.SetBool("groundBool", controller.isGrounded);

        RaycastHit hitInfo;

        if (controller.velocity.y < 0 && !controller.isGrounded)
        {
            Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity);

            if (hitInfo.distance > braceDistance)
            {
                animator.CrossFade("Falling", 0.5f);
            }
            else if (hitInfo.distance <= braceDistance)
            {
                animator.CrossFade("Bracing", 0.1f, 0, 0.4f);
            }
            Debug.Log(hitInfo.distance);

        }
    }

        void Jump()
    {
        if (controller.isGrounded)
        {
            transform.position += new Vector3(0, 0.051f, 0);
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
            animator.CrossFade("Jump", 0.1f, 0, 0.3f);
        }
    }



    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }

        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

}
