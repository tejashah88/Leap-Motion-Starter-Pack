using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class StateProcessor<T> {
    private StateChangeProcessor<T> scp;
    private StateEventProcessor<T> sep;

    public StateProcessor(StateChangeProcessor<T> _scp, StateEventProcessor<T> _sep) {
      scp = _scp;
      sep = _sep;
    }

    public void ProcessState(Hand hand) {
      scp.ProcessStateChange(hand);
      sep.ProcessState(scp.CurrentState.Get(), hand);
    }

    public T GetState() {
      return scp.CurrentState.Get();
    }
  }
}