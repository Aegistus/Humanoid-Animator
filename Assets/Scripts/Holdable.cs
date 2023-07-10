using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holdable : MonoBehaviour
{
    [SerializeField] Transform rightHandPosition;
    [SerializeField] Transform leftHandPosition;
    [Tooltip("How far away from the person should the object be held?")]
    [SerializeField] Vector3 offset;

    public Vector3 Offset => offset;
    public Transform RightHandPosition => rightHandPosition;
    public Transform LeftHandPosition => leftHandPosition;
}
