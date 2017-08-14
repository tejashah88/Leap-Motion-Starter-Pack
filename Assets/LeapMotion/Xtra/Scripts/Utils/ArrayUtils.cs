using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class ArrayUtils {
    public static T[] GetArrayFrom<T>(params T[] array) {
      return array;
    }

    public static T[] SubArray<T>(T[] data, int index, int length) {
      T[] result = new T[length];
      Array.Copy(data, index, result, 0, length);
      return result;
    }

    public static T Last<T>(T[] array) {
      return array[array.Length - 1];
    }
  }
}
