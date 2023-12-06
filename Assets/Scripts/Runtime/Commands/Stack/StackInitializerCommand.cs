using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class StackInitializerCommand
{
    private StackManager _stackManager;
    private GameObject _money;
    public StackInitializerCommand(StackManager stackManager,ref GameObject money)
    {
        _stackManager = stackManager;
        _money = money;
     
    }

    public void Execute()
    {
        //StackLevel'ý alýyor
        for (int i = 0; i < CoreGameSignals.Instance.onGetStackLevel?.Invoke(); i++)
        {
            //Stacklevel kadar money üretip collectableGameObjecte onlarý ekleyip tiplerini Update ediyor.
            GameObject obj = Object.Instantiate(_money);
            _stackManager.ItemAdderOnStackCommand.Execute(obj);
        }
        _stackManager.StackTypeUpdaterCommand.Execute();
    }
}
