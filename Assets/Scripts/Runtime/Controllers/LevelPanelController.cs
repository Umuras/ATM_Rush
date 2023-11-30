using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private int _moneyValue;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.Instance.onSetNewLevelValue += OnSetNewLevelValue;
        UISignals.Instance.onSetMoneyValue += OnSetMoneyValue;
        UISignals.Instance.onGetMoneyValue += OnGetMoneyValue;
    }

    private int OnGetMoneyValue()
    {
        return _moneyValue;
    }

    private void OnSetNewLevelValue(byte levelValue)
    {
        levelText.text = "LEVEL " + ++levelValue;
    }

    private void OnSetMoneyValue(int moneyValue)
    {
        _moneyValue = moneyValue;
        moneyText.text = moneyValue.ToString();
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.onSetNewLevelValue -= OnSetNewLevelValue;
        UISignals.Instance.onSetMoneyValue -= OnSetMoneyValue;
        UISignals.Instance.onGetMoneyValue -= OnGetMoneyValue;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
