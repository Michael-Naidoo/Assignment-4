using System;
using PlayerScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GuardScripts
{
    public class GuardMovement : MonoBehaviour
    {
        [Header("Input Data")]
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject player;
        [SerializeField] private NavMeshAgent navMesh;
        [SerializeField] private GameObject[] targets; 
        private float minDist = 20;
        public float crouchDist;
        public float walkDist;
        public float sprintDist;

        private PlayerMovement pm;

        [SerializeField] private int targetInArray;

        private void Start()
        {
            targetInArray = 0;
            player = GameObject.FindWithTag("Player");
            navMesh = GetComponent<NavMeshAgent>();
            pm = player.GetComponent<PlayerMovement>();
        }

        private void FixedUpdate()
        {
            if (pm.state == PlayerMovement.MovementState.crouching)
            {
                minDist = crouchDist;
            }
            else if(pm.state == PlayerMovement.MovementState.sprinting)
            {
                minDist = sprintDist;
            }
            else if (pm.state == PlayerMovement.MovementState.air)
            {
                minDist = sprintDist;
            }
            else if (pm.state == PlayerMovement.MovementState.walking)
            {
                minDist = walkDist;
            }
            var distFromPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distFromPlayer <= minDist)
            {
                target = player;
            }
            else
            {
                target = targets[targetInArray];
            }
            navMesh.destination = target.transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (targetInArray >= targets.Length - 1)
            {
                targetInArray = 0;
            }
            else
            {
                targetInArray++;
            }
        }
    }
}
