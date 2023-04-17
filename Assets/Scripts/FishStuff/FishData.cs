using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish/New Fish")]
public class FishData : ScriptableObject
{
   public GameObject prefab;

   public float weight;
   public float coolDown;

   [Title("AI")] public float fishSpeed;
   
   //Radius is used to check for obstacles
   public float sightRadius;
   //Avoid at all cost if distance is less than or equal to this value
   public float colliderRadius;
   //In case Agent has lost sight of target, we need to know where it was last seen. This will tell us if we are close enough
   public float targetReachedThreshold = 0.5f;
}
