using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonAiming : MonoBehaviour
{
    private Vector3 screenCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        screenCenter = Camera.main.ScreenToWorldPoint(new Vector3(0.5f,0.5f,0));
        transform.LookAt(screenCenter);
    }
}
