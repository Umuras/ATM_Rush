using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackTypeUpdaterCommand
{
    private List<GameObject> _collectableStack;
    private int _totalListScore;

    public StackTypeUpdaterCommand(ref List<GameObject> collectableStack)
    {
        _collectableStack = collectableStack;
    }

    public void Execute()
    {
        _totalListScore = 0;
        foreach (GameObject item in _collectableStack)
        {
            //_totalListScore += (int)item.GetComponent<CollectableManager>().CollectableTypeValue + 1;
        }

        ScoreSignals.Instance.onSetScore?.Invoke(_totalListScore);
    }
}