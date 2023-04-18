using System;
using System.Collections;
using System.Collections.Generic;
using FishStuff.Complex_AI;
using FishStuff.Complex_AI;
using FishStuff.Complex_AI.Behaviours;
using FishStuff.Complex_AI.Detectors;
using FishStuff.States;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent((typeof(AIData)))]
public class FishAI : MonoBehaviour
{
    [Title("Fish"), OnValueChanged("@ChangeAIData()")]
    public FishData fishInfo;


    [Title("AI")] [SerializeField] private AIData ai;

    //TODO: Move these to States, so they aren't running constantly
    [ReadOnly] private List<Detector> detectors;
    [ReadOnly] private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField] private float detectionDelay = 0.05f;

    [Title("States")] [ReadOnly, SerializeField]
    private State currentState;


    [SerializeField] private float fishSpeed;


    // Start is called before the first frame update

    private void Start()
    {
        detectors = new List<Detector>();
        steeringBehaviours = new List<SteeringBehaviour>();
        currentState = new WanderState();
        currentState.OnInitialized(this, ai);
      
        InvokeRepeating(nameof(PerformDetection), 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (var detector in detectors)
        {
            detector.Detect(aiData: ai);
        }
    }


    private void Update()
    {
        //Uptick state for every frame update
        currentState?.Update();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    #region AI

    public void AddBehaviour(SteeringBehaviour behaviour)
    {
        behaviour.OnStart(transform, ai);
        steeringBehaviours.Add(behaviour);
    }

    public void AddDetector(Detector detector)
    {
        detector.OnStart(transform, ai.obstaclesLayerMask);
        detectors.Add(detector);
    }

    public void RemoveEverything()
    {
        detectors.Clear();
        steeringBehaviours.Clear();
    }

    //Return the behaviours
    public List<SteeringBehaviour> GetSteeringBehaviours()
    {
        return steeringBehaviours;
    }


    public void ChangeAIData()
    {
        ai.sightRadius = fishInfo.sightRadius;
        //Get the width of the collider bounds
        var width = transform.GetComponent<CapsuleCollider>().radius;
        Debug.Log( transform.GetComponent<CapsuleCollider>());
        Debug.Log(width);
        ai.colliderRadius = width * 2f;
        ai.targetReachThreshold = fishInfo.targetReachedThreshold;
    }

    private void OnDrawGizmos()
    {
        for(int i = 5; i < Directions.EighteenDirections.Count; i++)
         Debug.DrawRay(transform.position,transform.TransformDirection(Directions.EighteenDirections[i]),Color.green);
    }

    #endregion
}