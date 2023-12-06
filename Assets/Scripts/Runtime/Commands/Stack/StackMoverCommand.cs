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
        //Burada ilk paranýn x konumu directionX konumuna doðru lerpSpeed hýzýnda ilerleyecek þekilde ayarlanýyor.
        float direct = Mathf.Lerp(collectableStack[0].transform.localPosition.x, directionX, _data.LerpSpeed);
        //Sonra ilk elemanýn pozisyonu buraya çekiliyor.
        collectableStack[0].transform.localPosition = new Vector3(direct, 0, 0);
        StackItemsLerpMove(collectableStack);
    }

    private void StackItemsLerpMove(List<GameObject> collectableStack)
    {
        for (int i = 1; i < collectableStack.Count; i++)
        {
            //Burada ikinci elemanýn pozisyon bilgisi alýnýyor.
            Vector3 pos = collectableStack[i].transform.localPosition;
            //Kendinden bir önceki elemanýn x ekseni konumu ona aktarýlýyor.
            pos.x = collectableStack[i - 1].transform.localPosition.x;
            //Kendinden bir öncekinin x konumuna doðru lerp iþlemi yapýlýyor.
            float direct = Mathf.Lerp(collectableStack[i].transform.localPosition.x, pos.x, _data.LerpSpeed);
            //Kendinden bir öncekinin konumuna taþýnýyor.
            collectableStack[i].transform.localPosition = new Vector3(direct, pos.y, pos.z);
        }
    }
}
