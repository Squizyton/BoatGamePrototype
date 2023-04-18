using System.Linq;
using UnityEngine;

namespace FishStuff.Complex_AI.Detectors
{
    public class ObstacleDetector : Detector
    {
        private Transform transform;
        
        //The range in which the AI can detect things
        [SerializeField] private float detectionRadius = 8.24f;

        private Collider[] colliders;

        public override void Detect(AIData aiData)
        {
            //TODO:: Change to NonAlloc
            colliders = Physics.OverlapSphere(_transform.position, aiData.sightRadius, _layerMask);
            aiData.obstacles = colliders;
        }
    }
}