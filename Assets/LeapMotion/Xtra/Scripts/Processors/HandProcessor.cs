using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class HandProcessor {
    private List<DefaultHandInterface> handInterfaces;

    private float currentTransitionLife = 0;

    private int prevHandCount;
    private int prevHandCountBeforeTransition;
    private bool isTransitioning;

    private float maxTransitionLife;
    private int maxHandCount;

    public HandProcessor(float mtl) {
      handInterfaces = new List<DefaultHandInterface>();
      maxTransitionLife = mtl;
      updateMaxHandCount();
    }

    public void Add(DefaultHandInterface handInterface) {
      if (handInterface != null)
        handInterfaces.Add(handInterface);
    }

    public void RemoveAt(int index) {
      handInterfaces.RemoveAt(index);
      updateMaxHandCount();
    }

    public void RemoveAll() {
      handInterfaces.RemoveAll(handInterface => true);
      updateMaxHandCount();
    }

    public void updateMaxHandCount() {
      maxHandCount = 0;

      if (handInterfaces.Count > 0) {
        foreach (DefaultHandInterface dhi in handInterfaces) {
          OneHandInterface ohi = dhi as OneHandInterface;
          TwoHandInterface thi = dhi as TwoHandInterface;

          if (ohi != null && maxHandCount < 1) maxHandCount = 1;
          if (thi != null && maxHandCount < 2) maxHandCount = 2;
        }
      }
    }

    void handleTransitionPhase(Frame frame, int hbtCount, float ctl, DefaultHandInterface dhi) {
      OneHandInterface ohi = dhi as OneHandInterface;
      TwoHandInterface thi = dhi as TwoHandInterface;

      switch (frame.Hands.Count) {
        case 0:
          if (hbtCount == 1 && ohi != null) ohi.OneToZero(ctl);
          if (hbtCount == 2 && thi != null) thi.TwoToZero(ctl);
          if (hbtCount > 2) dhi.TooManyHands();
          break;
        case 1:
          Hand currentHand = frame.Hands[0];
          if (hbtCount == 0 && ohi != null) ohi.ZeroToOne(currentHand, ctl);
          if (hbtCount == 2 && thi != null) thi.TwoToOne(currentHand, ctl);
          if (hbtCount > 2) dhi.TooManyHands();
          break;
        case 2:
          Hand[] currentHands = HandUtils.designateRightLeftHands(frame);
          if (hbtCount == 0 && thi != null) thi.ZeroToTwo(currentHands, ctl);
          if (hbtCount == 1 && thi != null) thi.OneToTwo(currentHands, ctl);
          if (hbtCount > 2) dhi.TooManyHands();
          break;
        default:
          dhi.TooManyHands();
          break;
      }
    }

    void handleStablePhase(Frame frame, DefaultHandInterface dhi) {
      OneHandInterface ohi = dhi as OneHandInterface;
      TwoHandInterface thi = dhi as TwoHandInterface;

      switch (frame.Hands.Count) {
        case 0:
          dhi.Zero();
          break;
        case 1:
          if (ohi != null) ohi.One(frame.Hands[0]);
          break;
        case 2:
          if (thi != null) thi.Two(HandUtils.designateRightLeftHands(frame));
          break;
        default:
          dhi.TooManyHands();
          break;
      }
    }

    void processFrame(Frame frame, DefaultHandInterface dhi) {
      int currentHandCount = frame.Hands.Count;

      if (currentHandCount != prevHandCount) { // start counting
        currentTransitionLife = 0;
        prevHandCountBeforeTransition = prevHandCount;
        isTransitioning = true;
      }

      if (isTransitioning) {
        if (currentTransitionLife < maxTransitionLife) {
          currentTransitionLife += Time.deltaTime;
          handleTransitionPhase(frame, prevHandCountBeforeTransition, currentTransitionLife, dhi);
        } else {
          isTransitioning = false;
        }
      } else {
        handleStablePhase(frame, dhi);
      }

      prevHandCount = currentHandCount;
    }

    public void ProcessFixedUpdate(Frame frame) {
      foreach(DefaultHandInterface handInterface in handInterfaces) {
        processFrame(frame, handInterface);
      }
    }
  }
}