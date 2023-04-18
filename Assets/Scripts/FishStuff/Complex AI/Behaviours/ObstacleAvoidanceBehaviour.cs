using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FishStuff.Complex_AI.Behaviours
{
    public class ObstacleAvoidanceBehaviour : SteeringBehaviour
    {
        private float[] dangersResultTemp = null;

      
        public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
        {
            if (aiData.obstacles == null) return (danger,interest);
            
            foreach (var obstacleCollider in aiData.obstacles)
            {
                Vector3 directionToObstacle;
                //Get the direction to the obstacle based on how close it is to the agent on the closest point
                if (obstacleCollider is TerrainCollider)
                {
                    var position = _transform.position;
                    NavMesh.SamplePosition(position, out var hit, 1f,NavMesh.AllAreas);
                    directionToObstacle = hit.position - position;

                }else
                {
                    var position = _transform.position;
                    directionToObstacle= obstacleCollider.ClosestPoint(position) - position;
                }

                var distanceToObstacle = directionToObstacle.magnitude;
                
                //Calculate weight based on the Distance between AI <----------> Obstacle
                var weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle) / radius;
                var directionNormalized = directionToObstacle.normalized;
                
                
                //ADd obstacle paremeters to the danger array
                for (var i = 0; i < Directions.EighteenDirections.Count; i++)
                {
                    //Get the angle between the direction to the obstacle and the direction of the current index
                    var result = Vector3.Dot(directionNormalized, Directions.EighteenDirections[i]);

                    var valueToPutIn = result * weight;

                    if (valueToPutIn > danger[i])
                        danger[i] = valueToPutIn;
                }
            }

            dangersResultTemp = danger;
            return (danger, interest);
        }
    }
    
    //Static class to store all the directions
    public static class Directions
    {
        public static readonly List<Vector3> EighteenDirections = new()
        {
            Vector3.up,
            Vector3.up + Vector3.forward,
            Vector3.up + Vector3.right,
            Vector3.up + Vector3.back,
            Vector3.up + Vector3.left, 
            Vector3.forward,
            Vector3.forward + Vector3.right,
            Vector3.right,
            Vector3.right + Vector3.back,
            Vector3.back,
            Vector3.back + Vector3.left,
            Vector3.left,
            Vector3.left + Vector3.forward,
            Vector3.down,
            Vector3.down + Vector3.forward,
            Vector3.down + Vector3.right,
            Vector3.down + Vector3.back,
            Vector3.down + Vector3.left
        };
    }
}