using System;
using System.Data;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] 
        private float moveSpeed;
        public float walkSpeed;
        public float sprintSpeed;

        public float groundDrag;

        [Header("Jumping")]
        public float jumpForce;
        public float jumpCooldown;
        public float airMultiplier;
        [SerializeField] private bool readyToJump;

        [Header("Crouching")] 
        public float crouchSpeed;
        public float crouchYScale;
        private float startYScale;
        [SerializeField] private bool canCrouch;
        
        [Header("KeyBinds")] 
        public KeyCode jumpKey = KeyCode.Space;
        public KeyCode sprintKey = KeyCode.LeftShift;    
        public KeyCode crouchKey = KeyCode.LeftControl;

        [Header("Ground Check")] 
        public float playerHeight;
        public LayerMask whatIsGround;
        private bool grounded;

        [Header("Slope Handling")] 
        public RaycastHit slopeHit;
        public float maxSlopeAngle;
        private bool exitingSlope;

        public Transform orientation;

        private float horizontalInput;
        private float verticalInput;

        private Vector3 moveDirection;

        private Rigidbody rb;

        public MovementState state;
        public enum MovementState
        {
            walking,
            sprinting,
            crouching,
            air
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            startYScale = transform.localScale.y;
        }

        private void Update()
        {
            //ground check
            grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
            
            MyInput();
            StateHandler();
            
            //handle drag
            if (grounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0f; 
            }
        }

        private void FixedUpdate()
        {
            MovePlayer();
            SpeedControl();
        }

        private void MyInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(jumpKey) && readyToJump && grounded)
            {
                readyToJump = false;
                
                Jump();
                
                Invoke(nameof(ResetJump), jumpCooldown);
            }

            if (Input.GetKey(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                if (canCrouch)
                {
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                    canCrouch = false;
                }
            }

            if (Input.GetKeyUp(crouchKey))
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                canCrouch = true;
            }
        }

        private void StateHandler()
        {
            //crouching
            if (Input.GetKey(crouchKey) && grounded)
            {
                state = MovementState.crouching;
                moveSpeed = crouchSpeed;
            }
            //sprinting
            else if (grounded && Input.GetKey(sprintKey))
            {
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
            }
            
            //walking
            else if (grounded)
            {
                state = MovementState.walking;
                moveSpeed = walkSpeed;
            }
            
            //air
            else
            {
                state = MovementState.air;
            }
        }
        private void MovePlayer()
        {
            // calculate movement direction
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (OnSlope() && !exitingSlope)
            {
                rb.AddForce(GetSlopeMoveDirection(moveDirection) * (moveSpeed * 20f), ForceMode.Force);

                if (rb.velocity.y > 0)
                {
                    rb.AddForce(Vector3.down * 80f, ForceMode.Force);
                }
            }
            if(grounded)
            {      
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
            }
            else if(!grounded)
            {
                rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
            }

            rb.useGravity = !OnSlope();
        }

        private void SpeedControl()
        {
            //Limiting Speed On Slope
            if (OnSlope() && !exitingSlope)
            {
                if (rb.velocity.magnitude > moveSpeed)
                {
                    rb.velocity = rb.velocity.normalized * moveSpeed;
                }
            }

            else
            {
                Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            
                //limit if needed
                if (flatVel.magnitude > moveSpeed)
                {
                    Vector3 limitedVel = flatVel.normalized * moveSpeed;
                    rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
                }
            }
        }

        private void Jump()
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            exitingSlope = true;
        }

        private void ResetJump()
        {
            readyToJump = true;

            exitingSlope = false;
        }

        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
            {
                float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        }
    }
}