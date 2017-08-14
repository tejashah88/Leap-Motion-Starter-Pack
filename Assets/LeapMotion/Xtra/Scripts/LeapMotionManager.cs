using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;


public class LeapMotionManager : MonoBehaviour {
  private LMProcessor lmb;
  public float maxTransitionLife = 0.5f;

  void Start() {
    lmb = new LMProcessor(maxTransitionLife);
    DefaultHandInterface[] handInterfaces = GetComponents<DefaultHandInterface>();
    Array.ForEach(handInterfaces, hInterface => lmb.AddHandInterface(hInterface));
  }

  void FixedUpdate() {
    lmb.ProcessTick();
  }
}