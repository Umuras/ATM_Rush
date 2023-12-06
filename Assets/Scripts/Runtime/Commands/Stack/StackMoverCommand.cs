using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMoverCommand
{
    private StackData _data;

    public StackMoverCommand(ref StackData stackData)
    {
        _data = stackData;
    }

    internal void Execute(float directionX, List<GameObject> collectableStack)
    {
        //Burada ilk paran�n x konumu directionX konumuna do�ru lerpSpeed h�z�nda ilerleyecek �ekilde ayarlan�yor.
        float direct = Mathf.Lerp(collectableStack[0].transform.localPosition.x, directionX, _data.LerpSpeed);
        //Sonra ilk eleman�n pozisyonu buraya �ekiliyor.
        collectableStack[0].transform.localPosition = new Vector3(direct, 0, 0);
        StackItemsLerpMove(collectableStack);
    }

    private void StackItemsLerpMove(List<GameObject> collectableStack)
    {
        for (int i = 1; i < collectableStack.Count; i++)
        {
            //Burada ikinci eleman�n pozisyon bilgisi al�n�yor.
            Vector3 pos = collectableStack[i].transform.localPosition;
            //Kendinden bir �nceki eleman�n x ekseni konumu ona aktar�l�yor.
            pos.x = collectableStack[i - 1].transform.localPosition.x;
            //Kendinden bir �ncekinin x konumuna do�ru lerp i�lemi yap�l�yor.
            float direct = Mathf.Lerp(collectableStack[i].transform.localPosition.x, pos.x, _data.LerpSpeed);
            //Kendinden bir �ncekinin konumuna ta��n�yor.
            collectableStack[i].transform.localPosition = new Vector3(direct, pos.y, pos.z);
        }
    }
}
