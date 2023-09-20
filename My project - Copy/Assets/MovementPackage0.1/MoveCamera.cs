using UnityEngine;

namespace MovementPackage0._1
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