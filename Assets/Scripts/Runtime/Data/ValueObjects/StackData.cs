using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StackData
{
    public float CollectableOffsetInStack;
    [Range(0.1f, 0.8f)]
    public float LerpSpeed;
    [Range(0, 02f)]
    public float StackAnimDuration;
    [Range(1f, 3f)]
    public float StackScaleValue;
    [Range(1f, 10f)]
    public float JumpForce;
    public float JumpItemsClampX;

    //public StackData(float collectableOffsetInStack, float lerpSpeed, float stackAnimDuration, float stackScaleValue, float jumpForce,
    //    float jumpItemsClampX)
    //{
    //    CollectableOffsetInStack = 1;
    //    LerpSpeed = 0.25f;
    //    StackAnimDuration = 0.12f;
    //    StackScaleValue = 1f;
    //    JumpForce = 7f;
    //    JumpItemsClampX = 5f;
    //}
}
