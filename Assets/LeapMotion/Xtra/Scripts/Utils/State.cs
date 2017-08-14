using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class State<T> {
    private T value;

    public State(T _value) {
      value = _value;
    }

    public T Get() {
      return value;
    }

    public void Set(T _value) {
      value = _value;
    }

    public bool SetIf(T compareState, T newState) {
      bool result = value.Equals(compareState);
      if (result) {
        Set(newState);
      }

      return result;
    }

    public bool SetIfAny(T[] compareStates, T newState) {
      foreach (T compareState in compareStates) {
        if (value.Equals(compareState)) {
          value = newState;
        }
      }

      return value.Equals(newState);
    }

    public bool SetIfAny(params T[] allStates) {
      T[] compareStates = ArrayUtils.SubArray(allStates, 0, allStates.Length - 2);
      T newState = ArrayUtils.Last(allStates);
      return SetIfAny(compareStates, newState);
    }

    public bool SetIfElse(T compareState, T firstNewState, T lastNewState) {
      bool result = value.Equals(compareState);
      Set(result ? firstNewState : lastNewState);
      return result;
    }

    public bool SetIfNot(T compareState, T newState) {
      bool result = !value.Equals(compareState);
      if (result) {
        Set(newState);
      }

      return result;
    }

    public bool SetIfNotElse(T compareState, T firstNewState, T lastNewState) {
      bool result = !value.Equals(compareState);
      Set(result ? firstNewState : lastNewState);
      return result;
    }

    public override bool Equals(object obj) {
      // Check for null values and compare run-time types.
      if (obj == null || typeof(T) != obj.GetType())
        return false;

      T tObj = (T) obj;
      return value.ToString().Equals(tObj.ToString());
    }

    public override int GetHashCode() {
      return value.GetHashCode();
    }
  }
}