using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    public UnityEvent eventA;

    public UnityEvent eventB;


    public void TriggerEventA()
    {
        eventA?.Invoke();
    }

    public void TriggerEventB()
    {
        eventB?.Invoke();
    }
}