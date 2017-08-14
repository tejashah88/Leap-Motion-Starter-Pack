# Leap-Motion-Starter-Pack (WIP)
A Unity library for jump starting your VR/AR project with Leap Motion integration.

# Table of Contents
* [Introduction](#introduction)
* [Documentation](#documentation)
* [Usage](#usage)
  * [Standard Libraries](#standard-libraries)
  * [Xtra Libraries](#xtra-libraries)
  * [Examples](#examples)
* [Changelog](#changelog)

# Introduction
This library contains all the necessary assets for building AR/VR-based apps with the Leap Motion. It includes [the standard assets provided by Leap Motion](https://developer.leapmotion.com/unity) as well as a couple utilities for even easier integration.

# Documentation
* Standard library
  * Core Assets - v4.2.1 [[docs](https://github.com/leapmotion/UnityModules/wiki/Core)]
  * Interaction Engine - v1.0.1 [[docs](https://github.com/leapmotion/UnityModules/wiki/Interaction-Engine)]
  * Hands Module - v2.1.2 [[docs](https://github.com/leapmotion/UnityModules/wiki/Hands-Module)]
  * Graphic Renderer - v0.1.0 [[docs](https://github.com/leapmotion/UnityModules/wiki/Graphic-Renderer)]
* C# Docs - v3.2 [[docs](https://developer.leapmotion.com/documentation/csharp/index.html)]
  * Hand class - [[docs](https://developer.leapmotion.com/documentation/csharp/api/Leap.Hand.html)]
  * Vector class - [[docs](https://developer.leapmotion.com/documentation/csharp/api/Leap.Vector.html)]

# Usage
## Standard Libraries
The standard libraries are located in [Assets/LeapMotion/Core](Assets/LeapMotion/Core) and [Assets/LeapMotion/Modules](Assets/LeapMotion/Modules). The "Core" folder has the barebone basics to implement Leap Motion integration while the "Modules" folder contains the extra libraries to allow you to interact with other objects, auto-rig different hand models, and optimize rendering calls. You can check out the respective documentation at the [Documentation](#documentation) section above.

## Xtra Libraries
The Xtra libraries contain a set of utility asssets (located in [Assets/LeapMotion/Xtra](Assets/LeapMotion/Xtra)), including prefabs and scripts to accelerate your VR/AR development. In the prefabs folder is an head-mounted VR rig that shows only hands (i.e. no arms) [in the capsule form](https://github.com/leapmotion/UnityModules/wiki/Core#capsule-hands).

This library was constructed with OOP/events in mind so in order to make the hands do anything, you'll have to create a separate class extending from either the `OneHandInterface` or the `TwoHandInterface` and attaching that as a component of the prefab. The interfaces themselves include methods to handle differents amounds of hands at well as transitioning states between different amounds of hands.

For implementing the `OneHandInterface`, you'll need to implement the following methods:
* Zero()
  * *fired when no hands are detected within the scene*
* TooManyHands()
  * *fired when too hands are detected within the scene*
* OneToZero(float transLife)
  * *fired when one of the hands has just left the scene*
* One(Hand presentHand)
  * *fired when one hand is detected within the scene*
* ZeroToOne(Hand futureHand, float transLife)
  * *fired when one hand has just entered the scene*

For implementing the `TwoHandInterface`, you'll need to implement the following **additional** methods:
* TwoToZero(float transLife)<sup>1</sup>
  * *fired when **exactly** both hands have just left the scene*
* TwoToOne(Hand futureHand, float transLife)
  * *fired when one of the hands has just left the scene*
* Two(Hand[] presentHands)
  * *fired when two hands are detected within the scene*
* ZeroToTwo(Hand[] futureHands, float transLife)<sup>1</sup>
  * *fired when **exactly** two hands have just entered the scene*
* OneToTwo(Hand[] futureHands, float transLife)
  * *fired when another hand has just entered the scene*

<sup>1</sup>: This event is rarely fired but it's best to implement a handler for it anyways.

There are two ways to go about implementing the said methods above. One is to use the provided utility classes named `State<T>`, `StateProcessor<T>`, `StateChangeProcessor<T>`, and `StateEventProcessor<T>`. With the `StateChangeProcessor<T>`, you can easily define simple rules for when a particular state should change and what state should it already be in order to change the state. With the `StateEventProcessor<T>`, you can also easily add state handling without worrying about setting up your own. The `StateProcessor<T>` helps to bind both classes into one nice package so that you can simply add the `ProcessState` method to any of the methods mentioned above. The other way is to implement state handling manually, if the provided classes aren't enough and you want more flexibility with your app.

As far as accessing the hand object properties, you can refer to the [Hand class](https://developer.leapmotion.com/documentation/csharp/api/Leap.Hand.html) and the [Vector class](https://developer.leapmotion.com/documentation/csharp/api/Leap.Vector.html) for more information (Note that Leap Motion's Vector class is **not** the same as [Unity's Vector3 class](https://docs.unity3d.com/ScriptReference/Vector3.html)).

## Examples

With Xtra libraries
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Xtra;

private enum OneHandState {
  IDLE,
  MOVING,
}

...

StateProcessor<OneHandState> oneHandStateProcessor;
public void Start() {
  StateChangeProcessor<OneHandState> oneHandScp = new StateChangeProcessor<OneHandState>();
  scp.NewRule((presentHand) => presentHand.PalmVelocity.Magnitude <= 0.5f, OneHandState.IDLE, OneHandState.MOVING);
  scp.NewRule((presentHand) => presentHand.PalmVelocity.Magnitude > 0.5f, OneHandState.MOVING, OneHandState.IDLE);
  
  StateEventProcessor<OneHandState> oneHandSep = new StateEventProcessor<OneHandState>();
  sep.OnState(OneHandState.IDLE, (presentHand) => Console.Log("The hand is idle."));
  sep.OnState(OneHandState.IDLE, (presentHand) => Console.Log("The hand is moving."));
  
  oneHandStateProcessor = new StateProcessor<OneHandState>(oneHandScp, oneHandSep);
}

...

public void One(Hand presentHand) {
  oneHandStateProcessor.ProcessState(presentHand);
}
```

Without Xtra libraries
```csharp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;

private enum OneHandState {
  IDLE,
  MOVING,
}

private OneHandState oneHandState;

...

StateProcessor<OneHandState> oneHandStateProcessor;
public void Start() {
  oneHandState = OneHandState.IDLE;
}

...

public void One(Hand presentHand) {
  if (presentHand.PalmVelocity.Magnitude <= 0.5f) {
    if (oneHandState == OneHandState.IDLE) {
      oneHandState = OneHandState.MOVING;
    }
  } else {
    if (oneHandState == OneHandState.MOVING) {
      oneHandState = OneHandState.IDLE;
    }
  }
  
  switch (oneHandState) {
    case OneHandState.IDLE:
      Console.Log("The hand is idle.");
      break;
    case OneHandState.MOVING:
      Console.Log("The hand is moving.");
      break;
  }
}
```

# Changelog
* v0.0.1 - First release
