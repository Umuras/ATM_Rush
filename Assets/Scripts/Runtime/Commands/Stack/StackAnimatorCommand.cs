using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackAnimatorCommand
{
    private StackManager _stackManager;
    private StackData _stackData;
    private List<GameObject> _collectableStack;

    public StackAnimatorCommand(StackManager stackManager, StackData stackData, ref List<GameObject> collectableStack)
    {
        _stackManager = stackManager;
        _stackData = stackData;
        _collectableStack = collectableStack;
    }

    public IEnumerator Execute()
    {
        for (int i = 0; i <= _collectableStack.Count - 1; i++)
        {
            //En son elemandan ba�lay�p ilk elemana kadar gidiyor, index hesab�nda.
            int index = (_collectableStack.Count - 1) - i;
            //StackScaleValue de�erince b�y�t�r, StackAnimDuration saniyesi kadar.
            _collectableStack[index].transform.DOScale(new Vector3(_stackData.StackScaleValue, _stackData.StackScaleValue, _stackData.StackScaleValue), _stackData.StackAnimDuration);
            //Tekrar  StackAnimDuration de�er saniyesi sonras� eski haline geliyor ve yine  StackAnimDuration saniyesi sonras� Ease.Flash uygulan�yor.
            _collectableStack[index].transform.DOScale(Vector3.one, _stackData.StackAnimDuration).SetDelay(_stackData.StackAnimDuration).SetEase(Ease.Flash);
            //Bu kadar zaman bekliyor.
            yield return new WaitForSeconds(_stackData.StackAnimDuration / 3);
        }
    }
}
