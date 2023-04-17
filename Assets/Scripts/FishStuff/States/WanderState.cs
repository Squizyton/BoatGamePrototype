using System.Linq;
using FishStuff.Complex_AI;
using FishStuff.Complex_AI.Behaviours;
using FishStuff.Complex_AI.Detectors;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

namespace FishStuff.States
{
    public class WanderState : State
    {
        private Transform _transform;
        
        private Transform wayPoint;

        private Collider[] hitColliders;
        private Collider[] cachedColliders;


        public override void OnInitialized(FishAI passedFish, AIData passedData)
        {
            fish = passedFish;
            aiData = passedData;

            _transform = fish.transform;

            fish.AddDetector(new ObstacleDetector());
            fish.AddDetector(new TargetDetector());
            fish.AddBehaviour(new ObstacleAvoidanceBehaviour());
            fish.AddBehaviour(new SeekBehaviour());
            contextSolver = new ContextSolver();
            aiData.GenerateNewTarget();

            hitColliders = new Collider[10];
        }

        public override void Update()
        {
            CheckDistanceFromCachedColliders();
            cachedColliders = new Collider[10];
        }

        public override void FixedUpdate()
        {

            //Slerp the rotation based on it's new forward vector
            _transform.rotation = Quaternion.Slerp(_transform.rotation,
                Quaternion.LookRotation(
                    _transform.forward + contextSolver.GetDirectionMove(fish.GetSteeringBehaviours(), aiData)),
                (Time.deltaTime * 5f));

            
            
           // _transform.position +=( _transform.forward +
          //      contextSolver.GetDirectionMove(fish.GetSteeringBehaviours(), aiData)) * (Time.deltaTime * fish.fishInfo.fishSpeed);

            //Clamp the y so the fish can't go above the water
            var pos = _transform.position;
            pos.y = Mathf.Clamp(_transform.position.y, -16f, 0);

            _transform.position = pos;
        }


        private void CheckDistanceFromCachedColliders()
        {
            if (cachedColliders == null) return;
            
            //Check distance from cached colliders
            for (var i = 0; i < cachedColliders.Length; i++)
            {
                if (!cachedColliders[i]) continue;
                
                if (!(Vector3.Distance(fish.transform.position, cachedColliders[i].transform.position) < fish.fishInfo.sightRadius)) continue;
                
                cachedColliders.ToList().RemoveAt(i);
            }
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}