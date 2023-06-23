using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HumanoidAnimator : MonoBehaviour
{
    [SerializeField] Transform aimTarget;
    [SerializeField] string fullBodyAnimationLayer = "Full Body";
    [SerializeField] string upperBodyAnimationLayer = "Upper Body";
    [SerializeField] float crossFadeTime = .2f;
    [SerializeField] AnimationEntry[] animationStates;
    [SerializeField] AnimationEntry[] upperBodyAnimationStates;

    Animator anim;
    int fullBodyLayerIndex;
    int upperBodyLayerIndex;

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

}
