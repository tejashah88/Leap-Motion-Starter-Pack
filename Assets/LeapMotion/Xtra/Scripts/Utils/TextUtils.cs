using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

namespace Leap.Unity.Xtra {
  public class TextUtils {
    public static string buildDebugText(params string[] texts) {
      string finalText = "";
      foreach (string text in texts) {
        finalText += text + "\n";
      }

      return finalText.Trim();
    }

    public static void setStatus(Text txtObj, string text, Color color) {
      txtObj.text = text;
      txtObj.color = color;
    }
  }
}
