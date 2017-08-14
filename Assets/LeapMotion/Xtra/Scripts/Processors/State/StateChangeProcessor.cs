using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class StateChangeProcessor<T> {
    public State<T> CurrentState { get; }
    private Dictionary<Func<Hand, bool>, Action> conditionToActionMap;

    public StateChangeProcessor(T defaultState) {
      CurrentState = new State<T>(defaultState);
      conditionToActionMap = new Dictionary<Func<Hand, bool>, Action>();
    }

    public StateChangeProcessor<T> NewRule(Func<Hand, bool> condition, T compareState, T finalState) {
      NewRule(condition, compareState, () => {
        CurrentState.Set(finalState);
      });

      return this;
    }

    public StateChangeProcessor<T> NewRule(Func<Hand, bool> condition, T[] compareStates, T finalState) {
      NewRule(condition, compareStates, () => {
        CurrentState.Set(finalState);
      });

      return this;
    }

    public StateChangeProcessor<T> NewRule(Func<Hand, bool> condition, T compareState, Action task) {
      NewRule(condition, () => {
        if (CurrentState.Equals(compareState)) {
          task();
        }
      });

      return this;
    }

    public StateChangeProcessor<T> NewRule(Func<Hand, bool> condition, T[] compareStates, Action task) {
      NewRule(condition, () => {
        foreach (T compareState in compareStates) {
          if (CurrentState.Equals(compareState)) {
            task();
            return;
          }
        }
      });

      return this;
    }

    public StateChangeProcessor<T> NewRule(Func<Hand, bool> condition, Action task) {
      conditionToActionMap.Add(condition, task);
      return this;
    }

    public void ProcessStateChange(Hand hand) {
      foreach(KeyValuePair<Func<Hand, bool>, Action> entry in conditionToActionMap) {
        if (entry.Key(hand)) {
          entry.Value();
          return;
        }
      }
    }
  }
}
