using System.Numerics;
using Sirenix.OdinInspector;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace FishStuff.Complex_AI.Behaviours
{
    public class SeekBehaviour: SteeringBehaviour
    {
        [Title("Thresholds")] private bool _reachedLastTarget = true;

        private Vector3 targetPositionCached;

        private float[] interestsTemp;
        
        
        
        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
        {
            
            //if we don't have a target stop seeking
            //else set a new Target
            
            //Cache the last position only if we still see the target (if the targets collection is not empty)
            targetPositionCached = aiData.currentTarget;

            //First check if we have reached the target
            if (Vector3.Distance(_transform.position, targetPositionCached) < targetReachedThreshold)
            {

                //TODO: Not really a todo, however uncomment if needed in future
                aiData.GenerateNewTarget();
                return (danger, interest);
            }
            
            
            //if we haven't yet reeach the target do the main logic of finding the interest directions
            var directionToTarget = (targetPositionCached - _transform.position);

            for (var i = 0; i < interest.Length; i++)
            {
                var result = Vector3.Dot(directionToTarget.normalized, Directions.EighteenDirections[i]);
                //Accept only directions at the less than 90 degrees from the target direction
                if (!(result > 0)) continue;
                
                if (result > interest[i])
                    interest[i] = result;
            }

            interestsTemp = interest;
            return (danger, interest);
        }
        
        
        
       
    }
}