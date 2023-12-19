using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickIncomeCommand
{
    private FeatureManager _featureManager;
    private int _newPriceTag;
    private byte _incomeLevel;

    public OnClickIncomeCommand(FeatureManager featureManager, ref int newPriceTag, ref byte incomeLevel)
    {
        _featureManager = featureManager;
        _newPriceTag = newPriceTag;
        _incomeLevel = incomeLevel;
    }

    internal void Execute()
    {
        _newPriceTag = (int)(ScoreSignals.Instance.onGetMoney?.Invoke() - ((Mathf.Pow(2, Mathf.Clamp(_incomeLevel, 0, 10)) * 100)));
        _incomeLevel += 1;
        _featureManager.SetIncomeLevel(_incomeLevel);
        ScoreSignals.Instance.onSendMoney?.Invoke((int)_newPriceTag);
        UISignals.Instance.onSetMoneyValue?.Invoke((int)_newPriceTag);
        _featureManager.SaveFeatureData();
    }
}
