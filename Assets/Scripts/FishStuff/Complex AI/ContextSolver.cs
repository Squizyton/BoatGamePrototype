using System;
using System.Collections.Generic;
using FishStuff.Complex_AI.Behaviours;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace FishStuff.Complex_AI
{
    
    public class ContextSolver
    {
        [Title("Debug")] private bool showGizmos = true;


        public float[] _interestGizmo = Array.Empty<float>();

        private Vector3 _resultDirection = Vector3.zero;


        private void Start()
        {
            _interestGizmo = new float[18];
        }

        public Vector3 GetDirectionMove(List<SteeringBehaviour> behaviours, AIData aiData)
        {
            var danger = new float[18];
            var interest = new float[18];
            
            //Loop through each behaviour
            foreach (var behaviour in behaviours)
            {
                (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
            }
            
            //Subtract danger values from interests array
            for (var i = 0; i < 18; i++)
            {
                //This removes diretions that we don't want to move in - 0 being don't move;
                interest[i] = Mathf.Clamp01(interest[i] - danger[i]);
            }

            _interestGizmo = interest;

            var outputDirection = Vector3.zero;

            //Get the average direction
            for (var i = 0; i < 18; i++)
            {
                //AFter we remove the
                outputDirection += Directions.EighteenDirections[i] * interest[i];
            }
            
            //Normalize the direction
            outputDirection.Normalize();
            //Set the result direction
            _resultDirection = outputDirection;

            //Return the direction
            return _resultDirection;
        }
        
        
    }
}