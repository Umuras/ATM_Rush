using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAdderOnStackCommand
{
    private StackManager _stackManager;
    private StackData _data;
    private List<GameObject> _collectableStack;

    public ItemAdderOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack, ref StackData stackData)
    {
        _stackManager = stackManager;
        _collectableStack = collectableStack;
        _data = stackData;
    }

    public void Execute(GameObject collectableGameObject)
    {
        if (_collectableStack.Count <= 0)
        {
            //Listede hi� gameobject olmad��� i�in ilk gameobjectimizi ekliyoruz.
            _collectableStack.Add(collectableGameObject);
            //Eklenilen gameobjecti StackManager gameobjectinin childi yap�yoruz.
            collectableGameObject.transform.SetParent(_stackManager.transform);
            //StackManager'� global vekt�r kabul edip managere g�re origin kabul edip kendi localPosition�m�z� 0,0,0 yap�yoruz.
            collectableGameObject.transform.localPosition = new Vector3(0,1f,0.335f);
        }
        else
        {
            //Gelen gameobjecti yine stackManager gameobjectinin childi yap�yoruz.
            collectableGameObject.transform.SetParent(_stackManager.transform);
            //Pozisyon olarak bu sefer listeye en son eklenen collectableGameObjectin pozisyonunu al�yoruz.
            //_collectableStack[_collectableStack.Count - 1] == _collectableStack[^1]
            Vector3 newPos = _collectableStack[_collectableStack.Count - 1].transform.localPosition;
            //ve z ekseninde belli mesefa �teliyoruz. Bunu yapmam�z�n sebebi ayn� konumda bulunurlarsa �st �ste binerler, ileri �teleyerek belli 
            //mesafe aral�k olu�turuyoruz.
            newPos.z += _data.CollectableOffsetInStack;
            //Yeni pozisyonumuzu ekliyoruz.
            collectableGameObject.transform.localPosition = newPos;
            //_collectableStacke objemizi ekliyoruz.
            _collectableStack.Add(collectableGameObject);
        }
    }
}
