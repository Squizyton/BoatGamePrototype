using System.Collections.Generic;
using UnityEngine;

namespace FishStuff.Complex_AI.Detectors
{
    public class TargetDetector : Detector
    {
        private float targetDetectionRange = 8.24f;

        private LayerMask playerLayerMask;
        [SerializeField] private List<Transform> colliders;

        public override void Detect(AIData aiData)
        {
            var playerCollider = Physics.OverlapSphere(_transform.position, targetDetectionRange, playerLayerMask);

            if (playerCollider.Length > 0)
            {
                var direction = (playerCollider[0].transform.position - _transform.position).normalized;

                Physics.Raycast(_transform.position, direction, out var hit, targetDetectionRange, _layerMask);

                if (hit.collider && (playerLayerMask & (1 << hit.collider.gameObject.layer)) != 0)
                    colliders = new List<Transform>() {playerCollider[0].transform};
                else colliders = null;
            }
            else colliders = null;

            aiData.targets = colliders;
        }
    }
}