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
        //CollectableStack i�indeki toplanabilir objelerin money, gold, diamond g�re score de�eri belirleniyor
        //money ise hepsi 0 = 0 + 0 + 1 bu +1 olmas�n�n sebebi moneynin de�eri 0 oldu�u i�in enumda ilk s�rada o y�zden
        //Hesaplama k�sm�nda 0 ile �arp�l�p scoreu 0 yazd�rmas�n diye sadece money varken ge�erli bu durum. Di�erleri i�in
        //ise 0 = 0 + 1 + 1 ilk eleman gold i�in diamond ise 0 = 0 + 2 + 1 �eklinde totalListScore collectableStack say�s�na ba�l� olarak
        //artacak ve sonra onSetScore'a ba�l� olan fonksiyon �al��t�r�l�p score belirlenecek.
        _totalListScore = 0;
        foreach (GameObject item in _collectableStack)
        {
            _totalListScore += item.GetComponent<CollectableManager>().GetCurrentValue() + 1;
        }

        ScoreSignals.Instance.onSetScore?.Invoke(_totalListScore);
    }
}
