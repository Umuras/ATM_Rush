using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField]
    private Animator animator;

    private float3 _initialPosition;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _initialPosition = transform.position;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onReset += OnReset;
        CameraSignals.Instance.onSetCinemachineTarget += OnSetCinemachineTarget;
        CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
    }

    private void OnSetCinemachineTarget(CameraTargetState state)
    {
        //switch (state) 
        //{
        //    case CameraTargetState.Player:
        //            Transform playerManager = FindAnyObjectByType<PlayerManager>().transform;
        //            stateDrivenCamera.Follow = playerManager;
        //            break;
        //    case CameraTargetState.FakePlayer:
        //        stateDrivenCamera.Follow = null;
        //        //Transform fakePlayer = FindAnyObjectByType<WallCheckController>().tranform.parent.transform;
        //        //stateDrivenCamera.Follow = fakePlayer;
        //        break;
        //    default:
        //        throw new ArgumentOutOfRangeException(nameof(state), state, null);
        //}
        Transform playerManager = FindObjectOfType<PlayerManager>().transform;
        stateDrivenCamera.Follow = playerManager;
    }

    private void OnChangeCameraState(CameraStates state)
    {
        animator.SetTrigger(state.ToString());
    }

    private void OnReset()
    {
        CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStates.Initial);
        stateDrivenCamera.Follow = null;
        stateDrivenCamera.LookAt = null;
        transform.position = _initialPosition;
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onReset -= OnReset;
        CameraSignals.Instance.onSetCinemachineTarget -= OnSetCinemachineTarget;
        CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }


}
