using System.Collections.Generic;
using System.Numerics;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject uiCanvas;
    public List<GameObject> environment;
    public BoxCollider wayPointBounds;

    public Transform debugSphere;
    public NavMeshSurface surface;
    public Transform water;
    public LayerMask environmentMask;
    public TerrainCollider terrainCollider;
    private void Awake()
    {
        instance = this;
    }

    public bool CheckForEnironmentCollision(Vector3 position)
    {
        foreach (var piece in environment)
        {
                
            piece.TryGetComponent(out Collider envCol);

            if (!envCol) return false;
                
            if (envCol.bounds.Contains(position))
                return true;
        }
        return false;
    }



    public void CanvasActive(bool value)
    
    {
        uiCanvas.SetActive(value);
    }

    public bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        NavMeshHit hit;
        
        for (int i = 0; i < 30; i++) {
            var randomPoint = center + Random.insideUnitSphere * range;
            if (!NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas) && hit.position.y > 0) continue;
            //shoot up a raycast from the hit position to make sure we don't collide  with any environment;

            var waterToGroundDifference = water.position.y - hit.position.y;

            //TODO: Change this so that if object was hit then the point of impact is max 
            if (Physics.Raycast(hit.position, Vector3.up, waterToGroundDifference, environmentMask)) continue;
            result = hit.position;

            var newPos = result;
            newPos.y = Random.Range(hit.position.y, 0);
            result = newPos;
            return true;

        }
        result = Vector3.zero;
        return false;
    }
    
    [ContextMenu("Test New Waypoint Method")]
    public void TestLocation()
    {
        Vector3 point;
        if (RandomPoint(debugSphere.transform.position, 10, out point))
        {
            debugSphere.transform.position = point;
            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
        }
    }
}