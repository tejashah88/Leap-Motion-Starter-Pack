using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class TwoHandInterface : OneHandInterface {
    public virtual void TwoToZero(float transLife) { PrintMissingMethodWarning(); }
    public virtual void TwoToOne(Hand futureHand, float transLife) { PrintMissingMethodWarning(); }
    public virtual void Two(Hand[] presentHands) { PrintMissingMethodWarning(); }
    public virtual void ZeroToTwo(Hand[] futureHands, float transLife) { PrintMissingMethodWarning(); }
    public virtual void OneToTwo(Hand[] futureHands, float transLife) { PrintMissingMethodWarning(); }
  }
}