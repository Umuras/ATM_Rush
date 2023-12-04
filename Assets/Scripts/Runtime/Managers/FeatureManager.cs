using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : MonoBehaviour
{
    private byte _incomeLevel = 1;
    private byte _stackLevel = 1;
    private int _newPriceTag;
    private readonly OnClickIncomeCommand _onClickIncomeCommand;
    private readonly OnClickStackCommand _onClickStackCommand;

    public FeatureManager()
    {
        _onClickIncomeCommand = new OnClickIncomeCommand(this, ref _newPriceTag, ref _incomeLevel);
        _onClickStackCommand = new OnClickStackCommand(this, ref _newPriceTag, ref _stackLevel);
    }

    private void Awake()
    {
        _incomeLevel = LoadIncomeData();
        _stackLevel = LoadStackData();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.Instance.onClickIncome += _onClickIncomeCommand.Execute;
        UISignals.Instance.onClickStack += _onClickStackCommand.Execute;
        CoreGameSignals.Instance.onGetIncomeLevel += OnGetIncomeLevel;
        CoreGameSignals.Instance.onGetStackLevel += OnGetStackLevel;
    }

    private byte OnGetIncomeLevel()
    {
        return _incomeLevel;
    }

    private byte OnGetStackLevel()
    {
        return _stackLevel;
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.onClickIncome -= _onClickIncomeCommand.Execute;
        UISignals.Instance.onClickStack -= _onClickStackCommand.Execute;
        CoreGameSignals.Instance.onGetIncomeLevel -= OnGetIncomeLevel;
        CoreGameSignals.Instance.onGetStackLevel -= OnGetStackLevel;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private byte LoadIncomeData()
    {
        if (!ES3.FileExists()) return 1;
        return (byte)(ES3.KeyExists("IncomeLevel") ? ES3.Load<int>("IncomeLevel") : 1);
    }

    private byte LoadStackData()
    {
        if (!ES3.FileExists()) return 1;
        return (byte)(ES3.KeyExists("StackLevel") ? ES3.Load<int>("StackLevel") : 1);
    }

    internal void SaveFeatureData()
    {
        SaveSignals.Instance.onSaveGameData?.Invoke();
    }
}
