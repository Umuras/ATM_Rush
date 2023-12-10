using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private PlayerMovementController movementController;
    [SerializeField]
    private PlayerAnimationController animationController;
    [SerializeField]
    private PlayerPhysicsController physicsController;
    [SerializeField]
    private PlayerMeshController meshController;

    private PlayerData _data;

    private const string PlayerDataPath = "Data/CD_Player";

    private void Awake()
    {
        _data = GetPlayerData();
        SendPlayerDataToControllers();    
    }

    private PlayerData GetPlayerData()
    {
       return Resources.Load<CD_Player>(PlayerDataPath).Data;
    }

    private void SendPlayerDataToControllers()
    {
        movementController.SetMovementData(_data.MovementData);
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        InputSignals.Instance.onInputTaken += OnActivateMovement;
        InputSignals.Instance.onInputReleased += OnDeactiveMovement;
        InputSignals.Instance.onInputDragged += OnInputDragged;
        CoreGameSignals.Instance.onPlay += OnPlay;
        CoreGameSignals.Instance.onLevelSuccessful += OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed += OnLevelFailed;
        CoreGameSignals.Instance.onReset += OnReset;
        PlayerSignals.Instance.onSetTotalScore += OnSetTotalScore;
        CoreGameSignals.Instance.onMiniGameEntered += OnMiniGameEntered;
    }

    private void OnPlay()
    {
        movementController.IsReadyToPlay(true);
        PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Run);
    }

    private void OnActivateMovement()
    {
        movementController.IsReadyToMove(true);
    }

    private void OnDeactiveMovement()
    {
        movementController.IsReadyToMove(false);
    }

    private void OnInputDragged(HorizontalInputParams inputParams)
    {
        movementController.UpdateInputValue(inputParams);
    }

    private void OnMiniGameEntered()
    {
        movementController.IsReadyToPlay(false);
        StartCoroutine(WaitForFinal());
    }

    private void OnSetTotalScore(int value)
    {
        //Burada normalde sinyal üzerinden yapýyorduk ama PlayerManager üzerinde PlayerSignals'ý tetikleyince orada dinlenen fonksiyon sürekli
        //sonsuz döngüye girip kendini tetikliyordu ve çalýþmýyordu, OnSetTotalScore fonksiyonunu internal yaparak direk nesne üzerinden
        //eriþimi saðladýk.
        //PlayerSignals.Instance.onSetTotalScore?.Invoke(value);
        meshController.OnSetTotalScore(value);
    }

    private void OnLevelSuccessful()
    {
        movementController.IsReadyToPlay(true);
    }

    private void OnLevelFailed()
    {
        movementController.IsReadyToPlay(false);
    }

    private void OnReset()
    {
        movementController.OnReset();
        animationController.OnReset();
    }

    private void UnSubscribeEvents()
    {
        InputSignals.Instance.onInputTaken -= OnActivateMovement;
        InputSignals.Instance.onInputReleased -= OnDeactiveMovement;
        InputSignals.Instance.onInputDragged -= OnInputDragged;
        CoreGameSignals.Instance.onPlay -= OnPlay;
        CoreGameSignals.Instance.onLevelSuccessful -= OnLevelSuccessful;
        CoreGameSignals.Instance.onLevelFailed -= OnLevelFailed;
        CoreGameSignals.Instance.onReset -= OnReset;
        PlayerSignals.Instance.onSetTotalScore -= OnSetTotalScore;
        CoreGameSignals.Instance.onMiniGameEntered -= OnMiniGameEntered;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    internal void SetStackPosition()
    {
        Vector3 position = transform.position;
        Vector2 pos = new Vector2(position.x, position.y);
        StackSignals.Instance.onStackFollowPlayer?.Invoke(pos);
    }

    private IEnumerator WaitForFinal()
    {
        //Karakter konveyöre geldiðinde idle konumuna geçiyor ve 2 saniye sonra PlayerManager gameobjtectinin görünürlüðü kapatýlýp
        //MiniGame baþlatýlýyor.
        PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationStates.Idle);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        CoreGameSignals.Instance.onMiniGameStart?.Invoke();
    }

}
