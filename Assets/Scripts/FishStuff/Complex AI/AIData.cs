using System;
using System.Collections.Generic;
using FishStuff.Complex_AI.Behaviours;
using Sirenix.OdinInspector;
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
        public Transform currentTarget;

        //Bounds
        private BoxCollider waypointColliderBox;
        
        
        [Title("Behaviour/Detector Variables")]
        //How far the fish can see
        public float sightRadius;
        //the colliders Radius
        public float colliderRadius;
        //How close the AI can be close to current target before condition is satisfied
        public float targetReachThreshold;
        public void FeedTarget(Transform newTarget)
        {
            currentTarget = newTarget;
        }

        private void Start()
        {
            waypointColliderBox = GameManager.instance.wayPointBounds;
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


            Debug.Log(GameManager.instance);
            if (!GameManager.instance.CheckForEnironmentCollision(point))
            {
                GameManager.instance.debugSphere.transform.position = waypointColliderBox.transform.TransformPoint(point);
                FeedTarget( GameManager.instance.debugSphere);
             
            }else GenerateNewTarget();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,sightRadius);
            
        }
    }
}
