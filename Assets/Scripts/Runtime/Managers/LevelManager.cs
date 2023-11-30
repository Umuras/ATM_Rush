using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelManager : MonoBehaviour
{
    public LevelManager()
    {
        _levelLoader = new LevelLoaderCommand(this);
        _levelDestroyer = new LevelDestroyerCommand(this);
    }

    [Header("Holder")]
    [SerializeField]
    internal GameObject levelHolder;

    [Space]
    [SerializeField]
    private byte totalLevelCount;

    private readonly LevelLoaderCommand _levelLoader;
    private readonly LevelDestroyerCommand _levelDestroyer;
    private GameData _gameData;

    private void Awake()
    {
        //AssignSaveData();
    }

    private void AssignSaveData()
    {
        _gameData.Level = SaveManager.GetSavedData();
    }

    private void OnEnable()
    {
        SubscribeEvents();
        //_gameData.Level = OnGetLevelID();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(0);
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize += _levelLoader.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyer.Execute;
        CoreGameSignals.Instance.onGetLevelID += OnGetLevelID;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }

    private byte OnGetLevelID()
    {
        return (byte)(0 % totalLevelCount);
    }

    private void OnNextLevel()
    {
        
        _gameData.Level++;
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        DOVirtual.DelayedCall(0.1f, () => CoreGameSignals.Instance.onLevelInitialize?.Invoke(OnGetLevelID()));
        CoreUISignals.Instance.onCloseAllPanels?.Invoke();
        //onLevelInitialize Sinyalinde zaten UIManagerde bütün panelleri açtýðý için burada bir daha açmamýz gerekmiyor.
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start,0);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 1);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 2);
    }

    private void OnRestartLevel()
    {
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        DOVirtual.DelayedCall(0.1f, () => CoreGameSignals.Instance.onLevelInitialize?.Invoke(OnGetLevelID()));
        CoreUISignals.Instance.onCloseAllPanels?.Invoke();
        //onLevelInitialize Sinyalinde zaten UIManagerde bütün panelleri açtýðý için burada bir daha açmamýz gerekmiyor.
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 0);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 1);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 2);
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize -= _levelLoader.Execute;
        CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyer.Execute;
        CoreGameSignals.Instance.onGetLevelID -= OnGetLevelID;
        CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
