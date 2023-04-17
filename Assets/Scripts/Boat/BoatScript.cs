using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

using Unity.VisualScripting;
using UnityEngine;

public class BoatScript : MonoBehaviour
{
    [SerializeField] public float boatSpeed;
    [SerializeField] public float fastboatSpeed;
    [SerializeField] public float currentBoatSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public CinemachineVirtualCamera aimingCamera;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private GameObject harpoon;

    private bool sprinting;
    private Vector3 movementInput;
    private BoatState state;

    private void Update()
    {
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");


        movementInput = new Vector3(y, 0, x);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            aimingCamera.Priority = 50;
            state = BoatState.Aiming;
            smokeParticles.Stop();
            GameManager.instance.CanvasActive(true);
            harpoon.SetActive(true);
            harpoon.GetComponent<Animator>().SetTrigger("appear");
        }else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (state != BoatState.Driving)
            {
                state = BoatState.Driving;
                aimingCamera.Priority = 0;
                smokeParticles.Play();
                harpoon.GetComponent<Animator>().SetTrigger("disapppear");
                GameManager.instance.CanvasActive(false);
            }
        }
        
        sprinting = Input.GetKey(KeyCode.LeftShift);
        currentBoatSpeed = sprinting ? fastboatSpeed : boatSpeed;

    }


    private void FixedUpdate()
    {
        switch (state)
        {
            case BoatState.Driving:
                if (movementInput.z != 0)
                    transform.Translate(0, 0, movementInput.z * currentBoatSpeed * Time.deltaTime);

                if (movementInput.x != 0)
                {
                    transform.Rotate(new Vector3(0, movementInput.x, 0) * (rotationSpeed * Time.deltaTime), Space.Self);
                }
                break;
        }


        // transform.Rotate(new Vector3(0, movementInput.y, 0) * (rotationSpeed * Time.deltaTime), Space.Self);
    }

    public enum BoatState
    {
        Driving,
        Aiming
    }
}