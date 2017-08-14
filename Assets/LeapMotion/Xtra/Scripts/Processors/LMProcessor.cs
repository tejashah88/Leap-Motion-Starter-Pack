using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

public class LMProcessor {
  private HandProcessor hmp;
  private LeapProvider provider;

  public HandProcessor GetHandProcessor() {
    return hmp;
  }

  public LeapProvider GetLeapProvider() {
    return provider;
  }

  public void AddHandInterface(DefaultHandInterface handInterface) {
    hmp.Add(handInterface);
  }

  public void RemoveAllHandInterfaces() {
    hmp.RemoveAll();
  }

  public LMProcessor(float maxTransLife) {
    hmp = new HandProcessor(maxTransLife);
    provider = MonoBehaviour.FindObjectOfType<LeapProvider>() as LeapProvider;
  }

	public void ProcessTick() {
    if (hmp != null && provider != null)
      hmp.ProcessFixedUpdate(provider.CurrentFrame);
	}
}
