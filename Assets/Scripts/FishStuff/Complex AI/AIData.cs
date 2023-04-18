using System;
using System.Collections.Generic;
using FishStuff.Complex_AI.Behaviours;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FishStuff.Complex_AI
{
    /// <summary>
    /// The purpose of this script is to hold data that AI-Based solving scripts can use to solve.
    /// </summary>
    public class AIData : MonoBehaviour
    {
        //Obstacles
        public LayerMask obstaclesLayerMask;

        //Obstacles that was found
        public Collider[] obstacles;

        //Targets detected by AI, in case we want multiple targets
        public List<Transform> targets = null;
        
        //AI's current target point
        public Vector3 currentTarget;

        //Bounds
        public BoxCollider waypointColliderBox;
        public CapsuleCollider thisCollider;
        
        [Title("Behaviour/Detector Variables")]
        //How far the fish can see
        public float sightRadius;
        //the colliders Radius
        public float colliderRadius;
        //How close the AI can be close to current target before condition is satisfied
        public float targetReachThreshold;
        public void FeedTarget(Vector3 newTarget)
        {
            currentTarget = newTarget;
        }

        private void Start()
        {
            Debug.Log($"Hello? : {GameManager.instance.wayPointBounds}");
            waypointColliderBox = GameManager.instance.wayPointBounds;
            thisCollider = GetComponent<CapsuleCollider>();
        }

        public void Intialize()
        {
            

        }

        public void GenerateNewTarget()
        {
          //Generate a new Point in the bounding
          Vector3 extents = waypointColliderBox.size / 2f;
          Vector3 point = new Vector3(
              Random.Range(-extents.x, extents.x),
              Random.Range(-extents.y, extents.y),
              Random.Range(-extents.z, extents.z)
          ) + waypointColliderBox.center;

          if (!GameManager.instance.CheckForEnironmentCollision(point))
          {
              FeedTarget(point);
          }else GenerateNewTarget();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,sightRadius);
            
        }
    }
}
