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
    [SerializeField] RuntimeAnimatorController controller;
    [SerializeField] Transform aimTarget;
    [SerializeField] string fullBodyAnimationLayer = "Full Body";
    [SerializeField] string upperBodyAnimationLayer = "Upper Body";
    [SerializeField] float crossFadeTime = .2f;

    Animator anim;
    int upperBodyLayerIndex;
    int[] fullBodyHashes;
    int[] upperBodyHashes;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        upperBodyLayerIndex = anim.GetLayerIndex(upperBodyAnimationLayer);
        fullBodyHashes = new int[Enum.GetValues(typeof(FullBodyAnimState)).Length];
        for (int i = 0; i < fullBodyHashes.Length; i++)
        {
            fullBodyHashes[i] = Animator.StringToHash(((FullBodyAnimState)i).ToString());
        }

        upperBodyHashes = new int[Enum.GetValues(typeof(UpperBodyAnimState)).Length];
        for (int i = 0; i < upperBodyHashes.Length; i++)
        {
            upperBodyHashes[i] = Animator.StringToHash(((UpperBodyAnimState)i).ToString());
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
    /// <param name="fullBodyAnimation">The state to play.</param>
    /// <param name="overwriteUpperBody">Whether this animation should overwrite the upper body animation as well.</param>
    public void PlayFullBodyAnimation(FullBodyAnimState fullBodyAnimation, bool overwriteUpperBody = true)
    {
        if (overwriteUpperBody)
        {
            anim.SetLayerWeight(upperBodyLayerIndex, 0);
        }
        int hash = fullBodyHashes[(int)fullBodyAnimation];
        anim.CrossFade(hash, crossFadeTime);
    }

    /// <summary>
    /// Play an upper body animation that is defined in the upperBodyAnimationStates array.
    /// </summary>
    /// <param name="upperBodyAnimation">The upper body state to play.</param>
    public void PlayUpperBodyAnimation(UpperBodyAnimState upperBodyAnimation)
    {
        int hash = upperBodyHashes[(int)upperBodyAnimation];
        anim.SetLayerWeight(upperBodyLayerIndex, 1);
        anim.CrossFade(hash, crossFadeTime, upperBodyLayerIndex);
    }
}
