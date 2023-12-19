using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        SaveSignals.Instance.onSaveGameData += OnSaveGameData;
    }

    private void OnSaveGameData()
    {
        Debug.LogError(ScoreSignals.Instance.onGetMoney());
        SaveData(new SaveGameDataParams()
        {
            Money = ScoreSignals.Instance.onGetMoney(),
            Level = CoreGameSignals.Instance.onGetLevelID(),
            IncomeLevel = CoreGameSignals.Instance.onGetIncomeLevel(),
            StackLevel = CoreGameSignals.Instance.onGetStackLevel()
        });
    }

    private void UnSubscribeEvents()
    {
        SaveSignals.Instance.onSaveGameData -= OnSaveGameData;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void SaveData(SaveGameDataParams saveGameDataParams)
    {
        ES3.Save("Level", saveGameDataParams.Level);
        ES3.Save("Money", saveGameDataParams.Money);
        ES3.Save("IncomeLevel", saveGameDataParams.IncomeLevel);
        ES3.Save("StackLevel", saveGameDataParams.StackLevel);

    }
}
