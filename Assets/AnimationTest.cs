using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    HumanoidAnimator humanoidAnimator;

    private void Awake()
    {
        humanoidAnimator = GetComponentInChildren<HumanoidAnimator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            humanoidAnimator.PlayAnimation(AnimationState.WalkForwards, false);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            humanoidAnimator.PlayAnimation(AnimationState.Idle, false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            humanoidAnimator.PlayUpperBodyAnimation(AnimationState.Punch);
        }
    }
}
