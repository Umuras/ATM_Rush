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
            //En son elemandan baþlayýp ilk elemana kadar gidiyor, index hesabýnda.
            int index = (_collectableStack.Count - 1) - i;
            //StackScaleValue deðerince büyütür, StackAnimDuration saniyesi kadar.
            _collectableStack[index].transform.DOScale(new Vector3(_stackData.StackScaleValue, _stackData.StackScaleValue, _stackData.StackScaleValue), _stackData.StackAnimDuration);
            //Tekrar  StackAnimDuration deðer saniyesi sonrasý eski haline geliyor ve yine  StackAnimDuration saniyesi sonrasý Ease.Flash uygulanýyor.
            _collectableStack[index].transform.DOScale(Vector3.one, _stackData.StackAnimDuration).SetDelay(_stackData.StackAnimDuration).SetEase(Ease.Flash);
            //Bu kadar zaman bekliyor.
            yield return new WaitForSeconds(_stackData.StackAnimDuration / 3);
        }
    }
}
