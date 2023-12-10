using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ItemRemoverOnStackCommand
{
    private StackManager _stackManager;
    private List<GameObject> _collectableStack;
    private Transform _levelHolder;

    public ItemRemoverOnStackCommand(StackManager stackManager, ref List<GameObject> collectableStack)
    {
        _stackManager = stackManager;
        _collectableStack = collectableStack;
        _levelHolder = GameObject.Find("LevelHolder").transform;
    }

    public void Execute(GameObject collectableGameObject)
    {
        //collectableGameObjectin _collectableStack i�erisinde hangi indexte yer ald���na eri�iyoruz.
        int index = _collectableStack.IndexOf(collectableGameObject);
        int lastIndex = _collectableStack.Count - 1;
        //B�yle yapmam�z�n sebebi collectableGameObject bir child objesi bu anda ondan kurtulup �yle yok ediyoruz, animasyon i�lemlerinde
        //childcount �zerinden i�lem yap�yoruz, orada problem ya�amamak i�in yap�yoruz.
        //collectableGameObject.transform.SetParent(null);
        //Bu �ekilde olunca levelHolder�n i�i level de�i�imi s�ras�nda yok edildi�i i�in otomatik olarak collectableGameObjectlerde siliniyor.
        collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
        collectableGameObject.SetActive(false);
        _stackManager.StackJumperCommand.Execute(lastIndex, index);
        _collectableStack.RemoveAt(index);
        _collectableStack.TrimExcess();
        _stackManager.StackTypeUpdaterCommand.Execute();
    }
}
