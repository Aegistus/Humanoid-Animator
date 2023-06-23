using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Hand
{
    Right, Left
}

public class HumanoidAnimator : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;
    [SerializeField] string fullBodyAnimationLayer = "Full Body";
    [SerializeField] string upperBodyAnimationLayer = "Upper Body";
    [SerializeField] float crossFadeTime = .2f;
    [SerializeField] AnimationEntry[] animationStates;
    [SerializeField] AnimationEntry[] upperBodyAnimationStates;

    Transform RightHandTarget { get; set; }
    Transform LeftHandTarget { get; set; }

    Animator anim;
    int fullBodyLayerIndex;
    int upperBodyLayerIndex;
    float rightHandIKWeight = 0f;
    float leftHandIKWeight = 0f;

    [System.Serializable]
    class AnimationEntry
    {
        public AnimationState state;
        [Tooltip("The name of the state in the animator controller.")]
        public string animatorStateName;
        [HideInInspector]
        public int animHash;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        fullBodyLayerIndex = anim.GetLayerIndex(fullBodyAnimationLayer);
        upperBodyLayerIndex = anim.GetLayerIndex(upperBodyAnimationLayer);
        for (int i = 0; i < animationStates.Length; i++)
        {
            animationStates[i].animHash = Animator.StringToHash(animationStates[i].animatorStateName);
        }
        for (int i = 0; i < upperBodyAnimationStates.Length; i++)
        {
            upperBodyAnimationStates[i].animHash = Animator.StringToHash(upperBodyAnimationStates[i].animatorStateName);
        }
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
    /// Sets the aim target's position relative to the humanoid.
    /// </summary>
    /// <param name="position">The position relative to the humanoid.</param>
    public void SetAimTargetPosition(Vector3 position)
    {
        aimTarget.position = position;
    }

    /// <summary>
    /// Play a humanoid animation that is defined in the animationStates array.
    /// </summary>
    /// <param name="animationState">The state to play.</param>
    /// <param name="overwriteUpperBody">Whether this animation should overwrite the upper body animation as well.</param>
    public void PlayAnimation(AnimationState animationState, bool overwriteUpperBody = true)
    {
        if (overwriteUpperBody)
        {
            anim.SetLayerWeight(upperBodyLayerIndex, 0);
        }
        int hash = Array.Find(animationStates, anim => anim.state == animationState).animHash;
        anim.CrossFade(hash, crossFadeTime);
    }

    /// <summary>
    /// Play an upper body animation that is defined in the upperBodyAnimationStates array.
    /// </summary>
    /// <param name="upperBodyAnimation">The upper body state to play.</param>
    public void PlayUpperBodyAnimation(AnimationState upperBodyAnimation)
    {
        int hash = Array.Find(upperBodyAnimationStates, anim => anim.state == upperBodyAnimation).animHash;
        anim.SetLayerWeight(upperBodyLayerIndex, 1);
        anim.CrossFade(hash, crossFadeTime, upperBodyLayerIndex);
    }

    /// <summary>
    /// Sets a target for either the left or right hand to reach for. 
    /// </summary>
    /// <param name="hand">Which hand to set the target for.</param>
    /// <param name="target">The target object to reach for.</param>
    /// <param name="grabSpeed">The speed at which the hand will initially move to the object.</param>
    public void SetHandTarget(Hand hand, Transform target, float grabSpeed)
    {
        if (hand == Hand.Right)
        {
            RightHandTarget = target;
            rightHandIKWeight = 0f;
        }
        else
        {
            LeftHandTarget = target;
            leftHandIKWeight = 0f;
        }
        StartCoroutine(IncreaseIKWeight(hand, grabSpeed));
    }

    // increases hand IK weight gradually to make arm movements less sudden.
    IEnumerator IncreaseIKWeight(Hand hand, float grabSpeed)
    {
        if (hand == Hand.Right)
        {
            while (rightHandIKWeight < 1)
            {
                rightHandIKWeight += grabSpeed * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (leftHandIKWeight < 1)
            {
                leftHandIKWeight += grabSpeed * Time.deltaTime;
                yield return null;
            }
        }
    }
}
