using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidIK : MonoBehaviour
{
    float rightHandIKWeight = 0f;
    float leftHandIKWeight = 0f;
    bool rightHandHasTarget = false;
    bool leftHandHasTarget = false;
    Animator anim;
    Transform RightHandTarget { get; set; }
    Transform LeftHandTarget { get; set; }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (RightHandTarget)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandIKWeight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.position);
            anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.rotation);
        }
        if (LeftHandTarget)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandIKWeight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation);
        }
    }

    /// <summary>
    /// Sets a target for either the left or right hand to reach for. 
    /// </summary>
    /// <param name="hand">Which hand to set the target for.</param>
    /// <param name="target">The target object to reach for.</param>
    /// <param name="grabSpeed">The speed at which the hand will initially move to the object.</param>
    public void SetHandTarget(Hand hand, Transform target, float grabSpeed = 1f)
    {
        if (hand == Hand.Right)
        {
            RightHandTarget = target;
            rightHandHasTarget = true;
            rightHandIKWeight = 0f;
        }
        else
        {
            LeftHandTarget = target;
            leftHandHasTarget = true;
            leftHandIKWeight = 0f;
        }
        StartCoroutine(IncreaseIKWeight(hand, grabSpeed));
    }

    // increases hand IK weight gradually to make arm movements less sudden.
    IEnumerator IncreaseIKWeight(Hand hand, float grabSpeed)
    {
        if (hand == Hand.Right)
        {
            while (rightHandIKWeight < 1 && rightHandHasTarget)
            {
                rightHandIKWeight += grabSpeed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (leftHandIKWeight < 1 && leftHandHasTarget)
            {
                leftHandIKWeight += grabSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Removes a hand's IK target and gradually moves the hand back to it's default position.
    /// </summary>
    /// <param name="hand">Which hand is being reset.</param>
    /// <param name="speed">How quickly the hand should move back to the default position</param>
    public void ResetHandIKTarget(Hand hand, float speed)
    {
        if (hand == Hand.Right)
        {
            rightHandHasTarget = false;
        }
        else
        {
            leftHandHasTarget = false;
        }
        StartCoroutine(DecreaseHandIKWeight(hand, speed));
    }

    IEnumerator DecreaseHandIKWeight(Hand hand, float speed)
    {
        if (hand == Hand.Right)
        {
            while (rightHandIKWeight > 0 && !rightHandHasTarget)
            {
                rightHandIKWeight -= speed * Time.deltaTime;
                yield return null;
            }
            RightHandTarget = null;
        }
        else
        {
            while (leftHandIKWeight > 0 && !leftHandHasTarget)
            {
                leftHandIKWeight -= speed * Time.deltaTime;
                yield return null;
            }
            LeftHandTarget = null;
        }
    }

    public void HoldItem(Transform item, Transform rightHandHold, Transform leftHandHold, Vector3 offset)
    {
        item.parent = transform;
        //item.position = transform.position;
        item.localPosition = offset;
        if (rightHandHold)
        {
            SetHandTarget(Hand.Right, rightHandHold);
        }
        if (leftHandHold)
        {
            SetHandTarget(Hand.Left, leftHandHold);
        }
    }
}
