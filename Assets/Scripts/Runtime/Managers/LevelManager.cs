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
    private byte _currentLevel;

    private void OnEnable()
    {
        SubscribeEvents();
        _currentLevel = OnGetLevelID();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(_currentLevel);
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
        if (!ES3.FileExists())
        {
            return 0;
        }
        else
        {
            return (byte)(ES3.KeyExists("Level") ? ES3.Load<int>("Level") % totalLevelCount : 0);
        }
    }

    private void OnNextLevel()
    {
        
        _currentLevel++;
        SaveSignals.Instance.onSaveGameData?.Invoke();
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(OnGetLevelID());
        //onLevelInitialize Sinyalinde zaten UIManagerde bütün panelleri açtýðý için burada bir daha açmamýz gerekmiyor.
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start,0);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 1);
        //CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Shop, 2);
    }

    private void OnRestartLevel()
    {
        SaveSignals.Instance.onSaveGameData?.Invoke();
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke(OnGetLevelID());
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
