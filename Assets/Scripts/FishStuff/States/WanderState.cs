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
        
        private LayerMask ground = LayerMask.NameToLayer("Terrain");

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
           // cachedColliders = new Collider[10];
           
           //TODO:Fix this. Idk why it doesn't work
            //if(Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.down),out hit ,Mathf.Infinity, ground));
            //{
              // Debug.Log(hit.collider);
              // Debug.Log(hit.point);
              // Debug.Log(hit.point.y);
                var pos = _transform.position;
                pos.y = Mathf.Clamp(pos.y, -10, 0);
                _transform.position = pos;
            //}
        }

        public override void FixedUpdate()
        {

            //Slerp the rotation based on it's new forward vector
            _transform.rotation = Quaternion.Slerp(_transform.rotation,
                Quaternion.LookRotation(
                    _transform.forward + contextSolver.GetDirectionMove(fish.GetSteeringBehaviours(), aiData)),
                (Time.deltaTime * 5f));


            var position = _transform.position;
            position +=( _transform.forward +
                         contextSolver.GetDirectionMove(fish.GetSteeringBehaviours(), aiData)) * (Time.deltaTime * fish.fishInfo.fishSpeed);



            _transform.position = position;

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