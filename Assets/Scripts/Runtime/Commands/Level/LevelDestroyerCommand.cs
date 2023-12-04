using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDestroyerCommand
{
    private LevelManager _levelManager;

    public LevelDestroyerCommand(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    public void Execute()
    {
        Object.Destroy(_levelManager.levelHolder.transform.GetChild(0).gameObject);
    }
}
