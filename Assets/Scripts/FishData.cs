using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish/New Fish")]
public class FishData : ScriptableObject
{
   public GameObject prefab;

   public float weight;
   public float coolDown;
}
