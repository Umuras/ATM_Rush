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
            //Listede hiç gameobject olmadýðý için ilk gameobjectimizi ekliyoruz.
            _collectableStack.Add(collectableGameObject);
            //Eklenilen gameobjecti StackManager gameobjectinin childi yapýyoruz.
            collectableGameObject.transform.SetParent(_stackManager.transform);
            //StackManager'ý global vektör kabul edip managere göre origin kabul edip kendi localPositionýmýzý 0,0,0 yapýyoruz.
            collectableGameObject.transform.localPosition = new Vector3(0,1f,0.335f);
        }
        else
        {
            //Gelen gameobjecti yine stackManager gameobjectinin childi yapýyoruz.
            collectableGameObject.transform.SetParent(_stackManager.transform);
            //Pozisyon olarak bu sefer listeye en son eklenen collectableGameObjectin pozisyonunu alýyoruz.
            //_collectableStack[_collectableStack.Count - 1] == _collectableStack[^1]
            Vector3 newPos = _collectableStack[_collectableStack.Count - 1].transform.localPosition;
            //ve z ekseninde belli mesefa öteliyoruz. Bunu yapmamýzýn sebebi ayný konumda bulunurlarsa üst üste binerler, ileri öteleyerek belli 
            //mesafe aralýk oluþturuyoruz.
            newPos.z += _data.CollectableOffsetInStack;
            //Yeni pozisyonumuzu ekliyoruz.
            collectableGameObject.transform.localPosition = newPos;
            //_collectableStacke objemizi ekliyoruz.
            _collectableStack.Add(collectableGameObject);
        }
    }
}
