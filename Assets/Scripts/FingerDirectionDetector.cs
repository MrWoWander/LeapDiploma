using UnityEngine;
using System.Collections;
using Leap.Unity.Attributes;

namespace Leap.Unity {

  public class FingerDirectionDetector : Detector {

    [Units("seconds")]
    [MinValue(0)]
    public float Period = .1f;

    public HandModelBase HandModel = null;  

    public Finger.FingerType FingerName = Finger.FingerType.TYPE_INDEX;

    [Header("Direction Settings")]
    public PointingType PointingType = PointingType.RelativeToHorizon;

    [DisableIf("PointingType", isEqualTo: PointingType.AtTarget)]
    public Vector3 PointingDirection = Vector3.forward;

    [DisableIf("PointingType", isNotEqualTo: PointingType.AtTarget)]
    public Transform TargetObject = null;

    [Range(0, 180)]
    public float OnAngle = 15f; 

    [Range(0, 180)]
    public float OffAngle = 25f;

    [Header("")]
    public bool ShowGizmos = true;

    private IEnumerator watcherCoroutine;

    private void OnValidate(){
      if( OffAngle < OnAngle){
        OffAngle = OnAngle;
      }
    }

    private void Awake () {
      watcherCoroutine = fingerPointingWatcher();
    }

    private void OnEnable () {
      StartCoroutine(watcherCoroutine);
    }
  
    private void OnDisable () {
      StopCoroutine(watcherCoroutine);
      Deactivate();
    }

    private IEnumerator fingerPointingWatcher() {
      Hand hand;
      Vector3 fingerDirection;
      Vector3 targetDirection;
      int selectedFinger = selectedFingerOrdinal();
      while(true){
        if(HandModel != null && HandModel.IsTracked){
          hand = HandModel.GetLeapHand();
          if(hand != null){
            targetDirection = selectedDirection(hand.Fingers[selectedFinger].TipPosition.ToVector3());
            fingerDirection = hand.Fingers[selectedFinger].Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToVector3();
            float angleTo = Vector3.Angle(fingerDirection, targetDirection);
            if(HandModel.IsTracked && angleTo <= OnAngle){
              Activate();
            } else if (!HandModel.IsTracked || angleTo >= OffAngle) {
              Deactivate();
            }
          }
        }
        yield return new WaitForSeconds(Period);
      }
    }

    private Vector3 selectedDirection(Vector3 tipPosition){
      switch(PointingType){
        case PointingType.RelativeToHorizon:
          Quaternion cameraRot = Camera.main.transform.rotation;
          float cameraYaw = cameraRot.eulerAngles.y;
          Quaternion rotator = Quaternion.AngleAxis(cameraYaw, Vector3.up);
          return rotator * PointingDirection;
        case PointingType.RelativeToCamera:
          return Camera.main.transform.TransformDirection(PointingDirection);
        case PointingType.RelativeToWorld:
          return PointingDirection;
        case PointingType.AtTarget:
          return TargetObject.position - tipPosition;
        default:
          return PointingDirection;
      }
    }

    private int selectedFingerOrdinal(){
      switch(FingerName){
        case Finger.FingerType.TYPE_INDEX:
          return 1;
        case Finger.FingerType.TYPE_MIDDLE:
          return 2;
        case Finger.FingerType.TYPE_PINKY:
          return 4;
        case Finger.FingerType.TYPE_RING:
          return 3;
        case Finger.FingerType.TYPE_THUMB:
          return 0;
        default:
          return 1;
      }
    }

  #if UNITY_EDITOR
    private void OnDrawGizmos () {
      if (ShowGizmos && HandModel != null && HandModel.IsTracked) {
        Color innerColor;
        if (IsActive) {
          innerColor = OnColor;
        } else {
          innerColor = OffColor;
        }
        Finger finger = HandModel.GetLeapHand().Fingers[selectedFingerOrdinal()];
        Vector3 fingerDirection = finger.Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToVector3();
        Utils.DrawCone(finger.TipPosition.ToVector3(), fingerDirection, OnAngle, finger.Length, innerColor);
        Utils.DrawCone(finger.TipPosition.ToVector3(), fingerDirection, OffAngle, finger.Length, LimitColor);
        Gizmos.color = DirectionColor;
        Gizmos.DrawRay(finger.TipPosition.ToVector3(), selectedDirection(finger.TipPosition.ToVector3()));
      }
    }
  #endif
  }
}
