using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class StateEventProcessor<T> {
    private Dictionary<T, Action<Hand>> stateHandlerMap;

    public StateEventProcessor() {
      stateHandlerMap = new Dictionary<T, Action<Hand>>();
    }

    public StateEventProcessor<T> OnState(T state, Action<Hand> stateHandler) {
      stateHandlerMap.Add(state, stateHandler);
      return this;
    }

    public void ProcessState(T state, Hand hand) {
      if (stateHandlerMap.ContainsKey(state))
        stateHandlerMap[state](hand);
    }

    public void ProcessState(StateChangeProcessor<T> scp, Hand hand) {
      ProcessState(scp.CurrentState.Get(), hand);
    }
  }
}