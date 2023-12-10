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
        //collectableGameObjectin _collectableStack içerisinde hangi indexte yer aldýðýna eriþiyoruz.
        int index = _collectableStack.IndexOf(collectableGameObject);
        int lastIndex = _collectableStack.Count - 1;
        //Böyle yapmamýzýn sebebi collectableGameObject bir child objesi bu anda ondan kurtulup öyle yok ediyoruz, animasyon iþlemlerinde
        //childcount üzerinden iþlem yapýyoruz, orada problem yaþamamak için yapýyoruz.
        //collectableGameObject.transform.SetParent(null);
        //Bu þekilde olunca levelHolderýn içi level deðiþimi sýrasýnda yok edildiði için otomatik olarak collectableGameObjectlerde siliniyor.
        collectableGameObject.transform.SetParent(_levelHolder.transform.GetChild(0));
        collectableGameObject.SetActive(false);
        _stackManager.StackJumperCommand.Execute(lastIndex, index);
        _collectableStack.RemoveAt(index);
        _collectableStack.TrimExcess();
        _stackManager.StackTypeUpdaterCommand.Execute();
    }
}
