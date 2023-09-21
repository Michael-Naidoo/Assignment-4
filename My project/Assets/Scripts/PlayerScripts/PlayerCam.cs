using UnityEngine;

namespace PlayerScripts
{
    public class PlayerCam : MonoBehaviour
    {
        public float sensX;
        public float sensY;

        public Transform orientation;

        private float xRotation;
        private float yRotation;

        private PlayerUI pUI;

        private void Start()
        {
            pUI = GameObject.FindWithTag("Player").GetComponent<PlayerUI>();

        }

        private void Update()
        {
            if (!pUI.pause)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (pUI.pause)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            //get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

            yRotation += mouseX;
        
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
            //rotate camera
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }
}
