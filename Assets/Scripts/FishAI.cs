using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private Vector3 newWayPoint;
    [SerializeField] private float fishSpeed;
    [SerializeField] private BoxCollider boundingBox;

    // Start is called before the first frame update
    void Start()
    {
        boundingBox = GameObject.Find("WaypointBoundingBox").GetComponent<BoxCollider>();
        GenerateNewWayPoint();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(newWayPoint);

        var step = fishSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, newWayPoint, step);

        
        transform.LookAt(newWayPoint);
        
        if (Vector3.Distance(transform.position, newWayPoint) < 1f)
        {
            GenerateNewWayPoint();
        }
    }


    void GenerateNewWayPoint()
    {
        //Generate a new Point in the bounding
        Vector3 extents = boundingBox.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        ) + boundingBox.center;


        Debug.Log(GameManager.instance);
        if (!GameManager.instance.CheckForEnironmentCollision(point))
        {
            newWayPoint = boundingBox.transform.TransformPoint(point);
        }else GenerateNewWayPoint();
    }
}