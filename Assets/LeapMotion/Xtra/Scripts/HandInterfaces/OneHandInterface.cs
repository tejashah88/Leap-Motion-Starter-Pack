using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class OneHandInterface : DefaultHandInterface {
    public virtual void OneToZero(float transLife) { PrintMissingMethodWarning(); }
    public virtual void One(Hand presentHand) { PrintMissingMethodWarning(); }
    public virtual void ZeroToOne(Hand futureHand, float transLife) { PrintMissingMethodWarning(); }
  }
}
