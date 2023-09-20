using System;
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
        [SerializeField] private float minDist;

        [SerializeField] private int targetInArray;

        private void Start()
        {
            targetInArray = 0;
            player = GameObject.FindWithTag("Player");
            navMesh = GetComponent<NavMeshAgent>();
        }

        private void FixedUpdate()
        {
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
