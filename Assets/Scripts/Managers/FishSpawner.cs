using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FishStuff.Complex_AI;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
   [SerializeField] public List<FishData> fishes = new List<FishData>();
   [SerializeField] private float spawnRadius;

   [SerializeField] private Transform baseSpawnPoint;
   [SerializeField] private float rayLength;
   [SerializeField] private Transform water;
   [SerializeField] private LayerMask enviroMask;

   private const int MAX_FISH = 50;
   private int currentFish;
   private void Start()
   {
      SpawnAFish();
      
   }


   void SpawnAFish()
   {
      if (currentFish >= MAX_FISH) return;
      

      var fish = ReturnAFish();
      
      var xPos = Mathf.Cos(Mathf.Deg2Rad * Random.Range(0, 360)) * Random.Range(0,spawnRadius) + baseSpawnPoint.position.x;
      var zPos = Mathf.Sin(Mathf.Deg2Rad * Random.Range(0, 360)) * Random.Range(0,spawnRadius) + baseSpawnPoint.position.z;

      if (!Physics.Raycast(new Vector3(xPos,transform.position.y,zPos), Vector3.down, out RaycastHit hit, rayLength,enviroMask))
      {

         var y = Random.Range(-16, water.position.y);

         var spawnedFish = Instantiate(fish.prefab, new Vector3(xPos, y, zPos), Quaternion.identity);
         
         //TODO:Remove this when you fix the null bug
         spawnedFish.TryGetComponent(out AIData aiData);
         aiData.waypointColliderBox = GameManager.instance.wayPointBounds;
         
         
         StartCoroutine(Cooldown(fish.coolDown));
         currentFish++;
      }
      else
      {
         StartCoroutine(Cooldown(.1f));
      }
      // var pos = new Vector3(xPos,yPos,zPos);
   }

   IEnumerator Cooldown(float time)
   {
      yield return new WaitForSeconds(time);
      SpawnAFish();
   }

   private FishData ReturnAFish()
   {
      //make a temporary list of monsters
      var tempList = fishes;
        
      //shuffle the list
      tempList = tempList.OrderBy(x => Random.value).ToList();

      var weightSum = 0f;

      var weightIndex = new List<float>();
        
        
      //loop through the collection of available monsters and grab each weight
      foreach (var monster in tempList.Where(monster => monster.weight > 0))
      {
         weightSum += monster.weight;
         weightIndex.Add(weightSum);
      }

      var index = 0;
      var lastIndex = fishes.Count - 1;

      while (index < lastIndex)
      {
         //Do a probability check with a likelihood of weight. The greater the number, the greater the more likely its to spawn
         if (Random.Range(0, weightSum) < weightIndex[index])
         {
            return tempList[index];
         }
            
         //Remove the last item from the sum of total untested weights and try again
         weightSum -= weightIndex[index];

         index++;
      }

      return tempList[index];
   }
   
   
   public void OnDrawGizmos()
   {
      Handles.color = Color.red;
      Handles.DrawWireDisc(baseSpawnPoint.transform.position, new Vector3(0, 1, 0),spawnRadius);

      
      Debug.DrawRay(transform.position,transform.TransformDirection(Vector3.down) * rayLength,Color.red);
   }
}
