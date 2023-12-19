using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        PlayerSignals.Instance.onChangePlayerAnimationState += OnChangeAnimationState;
    }
    //Karakterin bulunduðu duruma göre animasyonu deðiþtiriliyor. Idle, Run, Happy þeklinde. Happy MiniGamede çalýþýyor.
    private void OnChangeAnimationState(PlayerAnimationStates animationState)
    {
        animator.SetTrigger(animationState.ToString());
    }

    private void UnSubscribeEvents()
    {
        PlayerSignals.Instance.onChangePlayerAnimationState -= OnChangeAnimationState;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    internal void OnReset()
    {
        PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Idle);
    }
}
