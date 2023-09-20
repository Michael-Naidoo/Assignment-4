using UnityEngine;

namespace PlayerScripts
{
    public class Sliding : MonoBehaviour
    {
          [Header("References")] 
        public Transform orientation;
        public Transform playerObj;
        private Rigidbody rb;
        private PlayerMovement pm;

        [Header("Sliding")] 
        public float maxSlideTime;
        public float slideForce;
        private float slideTimer;
        [SerializeField] private bool canSlide;

        public float slideYScale;
        private float startYScale;
    
        [Header("Input")]
        public KeyCode slideKey = KeyCode.LeftControl;
        private float horizontalInput;
        private float VerticalInput;

        private bool sliding;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            pm = GetComponent<PlayerMovement>();

            startYScale = playerObj.transform.localScale.y;
        }

        private void Update()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || VerticalInput != 0))
            {
                StartSlide();
            }

            if (Input.GetKeyUp(slideKey) && sliding)
            {
                StopSlide();
            }
        }

        private void FixedUpdate()
        {
            if (sliding)
            {
                SlidingMovement();
            }
        }

        private void StartSlide()
        {
            sliding = true;

            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            if (canSlide)
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                canSlide = false;
            }

            slideTimer = maxSlideTime;
        }

        private void SlidingMovement()
        {
            Vector3 inputDirection = orientation.forward * VerticalInput + orientation.right * horizontalInput;
        
        

            if (!pm.OnSlope() || rb.velocity.y > -0.1f)
            {
                rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

                slideTimer -= Time.deltaTime;
            }
            else
            {
                rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            }
        
            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }
    
        private void StopSlide()
        {
            sliding = false;
        
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

            canSlide = true;
        }
    }
}
