using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] Transform targetObject;

    HumanoidAnimator humanoidAnimator;
    HumanoidIK ik;

    private void Awake()
    {
        humanoidAnimator = GetComponentInChildren<HumanoidAnimator>();
        ik = GetComponentInChildren<HumanoidIK>();
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

        if (Input.GetKeyDown(KeyCode.G))
        {
            ik.SetHandTarget(Hand.Right, targetObject, 1f);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            ik.ResetHandIKTarget(Hand.Right, 1f);
        }
    }

    
}
