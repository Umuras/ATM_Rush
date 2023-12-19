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
        //CollectableStack içindeki toplanabilir objelerin money, gold, diamond göre score deðeri belirleniyor
        //money ise hepsi 0 = 0 + 0 + 1 bu +1 olmasýnýn sebebi moneynin deðeri 0 olduðu için enumda ilk sýrada o yüzden
        //Hesaplama kýsmýnda 0 ile çarpýlýp scoreu 0 yazdýrmasýn diye sadece money varken geçerli bu durum. Diðerleri için
        //ise 0 = 0 + 1 + 1 ilk eleman gold için diamond ise 0 = 0 + 2 + 1 þeklinde totalListScore collectableStack sayýsýna baðlý olarak
        //artacak ve sonra onSetScore'a baðlý olan fonksiyon çalýþtýrýlýp score belirlenecek.
        _totalListScore = 0;
        foreach (GameObject item in _collectableStack)
        {
            _totalListScore += item.GetComponent<CollectableManager>().GetCurrentValue() + 1;
        }

        ScoreSignals.Instance.onSetScore?.Invoke(_totalListScore);
    }
}
