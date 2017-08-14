using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;

namespace Leap.Unity.Xtra {
  public class DefaultHandInterface : MonoBehaviour {
    public bool strict;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private string GetCurrentMethod() { return (new System.Diagnostics.StackTrace()).GetFrame(2).GetMethod().Name; }

    protected void PrintMissingMethodWarning() {
      if (strict) Debug.LogWarningFormat("'{0}' method not implemented.", GetCurrentMethod());
    }

    public virtual void Zero() { PrintMissingMethodWarning(); }
    public virtual void TooManyHands() { PrintMissingMethodWarning(); }
  }
}