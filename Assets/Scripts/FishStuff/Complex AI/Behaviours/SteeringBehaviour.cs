using UnityEngine;

namespace FishStuff.Complex_AI.Behaviours
{
    public  abstract class SteeringBehaviour
    {
        //Radius is used to check for obstacles: same as the radius of the opbstacle detection
        protected float radius;
        
        //Avoid at all cost if distance is less than or equal to this value -> set value automatically based on current collider of object
        protected float agentColliderSize;
        
        //In case Agent has lost sight of target, we need to know where it was last seen. This will tell us if we are close enough
        protected float targetReachedThreshold;

        protected Transform _transform;

        public virtual void OnStart(Transform passedTransform, AIData aiData)
        {
            radius = aiData.sightRadius;
            agentColliderSize = aiData.colliderRadius;
            targetReachedThreshold = aiData.targetReachThreshold;
            _transform = passedTransform;
        }

        
        //Pass in to receive direction

        public abstract (float[] danger, float[]interest) GetSteering(float[] danger, float[] interest, AIData aiData);

    }
}