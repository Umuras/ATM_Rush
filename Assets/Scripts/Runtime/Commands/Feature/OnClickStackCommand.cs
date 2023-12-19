using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickStackCommand
{
    private FeatureManager _featureManager;
    private int _newPriceTag;
    private byte _stackLevel;

    public OnClickStackCommand(FeatureManager featureManager, ref int newPriceTag, ref byte stackLevel)
    {
        _featureManager = featureManager;
        _newPriceTag = newPriceTag;
        _stackLevel = stackLevel;
    }

    internal void Execute()
    {
        _newPriceTag = (int)(ScoreSignals.Instance.onGetMoney?.Invoke() - ((Mathf.Pow(2, Mathf.Clamp(_stackLevel, 0,10))*100)));
        _stackLevel += 1;
        ScoreSignals.Instance.onSendMoney?.Invoke((int)_newPriceTag);
        UISignals.Instance.onSetMoneyValue?.Invoke((int)_newPriceTag);
        _featureManager.SaveFeatureData();
    }
}
