using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    [SerializeField] Transform targetObject;
    [SerializeField] Holdable gun;

    HumanoidAnimator humanoidAnimator;
    HumanoidIK ik;

    private void Awake()
    {
        humanoidAnimator = GetComponentInChildren<HumanoidAnimator>();
        ik = GetComponentInChildren<HumanoidIK>();
        print(FullBodyAnimState.Idle.ToString());
        print(FullBodyAnimState.WalkForwards.ToString());
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            humanoidAnimator.PlayFullBodyAnimation(FullBodyAnimState.WalkForwards, false);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            humanoidAnimator.PlayFullBodyAnimation(FullBodyAnimState.Idle, false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            humanoidAnimator.PlayUpperBodyAnimation(UpperBodyAnimState.Punch);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ik.SetHandTarget(Hand.Right, targetObject, 1f);
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            ik.ResetHandIKTarget(Hand.Right, 1f);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            ik.HoldItem(gun.transform, gun.RightHandPosition, gun.LeftHandPosition, gun.Offset);
        }
    }

    
}
