using System;
using UnityEngine;

namespace DefaultNamespace
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