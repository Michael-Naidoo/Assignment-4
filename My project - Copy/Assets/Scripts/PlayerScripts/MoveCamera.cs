using UnityEngine;

namespace PlayerScripts
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        private void Update()
        {
            transform.position = cameraPosition.transform.position;
        }
    }
}
