using UnityEngine;
using System.Collections;
using System;
using Leap.Unity.Attributes;

namespace Leap.Unity {

  public class ExtendedFingerDetector : Detector {

    [Units("seconds")]
    [MinValue(0)]
    public float Period = .1f;

    public HandModelBase HandModel = null;
  
    [Header("Finger States")]
    public PointingState Thumb = PointingState.Either;
    public PointingState Index = PointingState.Either;
    public PointingState Middle = PointingState.Either;
    public PointingState Ring = PointingState.Either;
    public PointingState Pinky = PointingState.Either;


    [Header("Min and Max Finger Counts")]
    [Range(0,5)]
    public int MinimumExtendedCount = 0;

    [Range(0, 5)]
    public int MaximumExtendedCount = 5;

    [Header("")]
    public bool ShowGizmos = true;

    private IEnumerator watcherCoroutine;

    void OnValidate() {
      int required = 0, forbidden = 0;
      PointingState[] stateArray = { Thumb, Index, Middle, Ring, Pinky };
      for(int i=0; i<stateArray.Length; i++) {
        var state = stateArray[i];
        switch (state) {
          case PointingState.Extended:
            required++;
            break;
          case PointingState.NotExtended:
            forbidden++;
            break;
          default:
            break;
        }
        MinimumExtendedCount = Math.Max(required, MinimumExtendedCount);
        MaximumExtendedCount = Math.Min(5 - forbidden, MaximumExtendedCount);
        MaximumExtendedCount = Math.Max(required, MaximumExtendedCount);
      }
    
    }

    void Awake () {
      watcherCoroutine = extendedFingerWatcher();
    }
  
    void OnEnable () {
      StartCoroutine(watcherCoroutine);
    }
  
    void OnDisable () {
      StopCoroutine(watcherCoroutine);
      Deactivate();
    }
  
    IEnumerator extendedFingerWatcher() {
      Hand hand;
      while(true){
        bool fingerState = false;
        if(HandModel != null && HandModel.IsTracked){
          hand = HandModel.GetLeapHand();
          if(hand != null){
            fingerState = matchFingerState(hand.Fingers[0], Thumb)
              && matchFingerState(hand.Fingers[1], Index)
              && matchFingerState(hand.Fingers[2], Middle)
              && matchFingerState(hand.Fingers[3], Ring)
              && matchFingerState(hand.Fingers[4], Pinky);

            int extendedCount = 0;
            for (int f = 0; f < 5; f++) {
              if (hand.Fingers[f].IsExtended) {
                extendedCount++;
              }
            }
            fingerState = fingerState && 
                         (extendedCount <= MaximumExtendedCount) && 
                         (extendedCount >= MinimumExtendedCount);
            if(HandModel.IsTracked && fingerState){
              Activate();
            } else if(!HandModel.IsTracked || !fingerState) {
              Deactivate();
            }
          }
        } else if(IsActive){
          Deactivate();
        }
        yield return new WaitForSeconds(Period);
      }
    }

    private bool matchFingerState (Finger finger, PointingState requiredState) {
      return (requiredState == PointingState.Either) ||
             (requiredState == PointingState.Extended && finger.IsExtended) ||
             (requiredState == PointingState.NotExtended && !finger.IsExtended);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos () {
      if (ShowGizmos && HandModel != null && HandModel.IsTracked) {
        PointingState[] state = { Thumb, Index, Middle, Ring, Pinky };
        Hand hand = HandModel.GetLeapHand();
        int extendedCount = 0;
        int notExtendedCount = 0;
        for (int f = 0; f < 5; f++) {
          Finger finger = hand.Fingers[f];
          if (finger.IsExtended) extendedCount++;
          else notExtendedCount++;
          if (matchFingerState(finger, state[f]) && 
             (extendedCount <= MaximumExtendedCount) && 
             (extendedCount >= MinimumExtendedCount)) {
            Gizmos.color = OnColor;
          } else {
            Gizmos.color = OffColor;
          }
          Gizmos.DrawWireSphere(finger.TipPosition.ToVector3(), finger.Width);
        }
      }
    }
    #endif
  }
  
  public enum PointingState{Extended, NotExtended, Either}
}
