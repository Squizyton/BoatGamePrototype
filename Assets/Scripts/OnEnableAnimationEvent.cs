using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableAnimationEvent : MonoBehaviour
{
   public string eventTrigger;
   public Animator anim;
   private void OnEnable()
   {
      anim.SetTrigger(eventTrigger);
   }
}
