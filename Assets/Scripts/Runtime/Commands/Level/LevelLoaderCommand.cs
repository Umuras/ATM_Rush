using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelLoaderCommand
{
    private LevelManager _levelManager;

    public LevelLoaderCommand(LevelManager levelManager)
    {
        _levelManager = levelManager;
    }

    public void Execute(byte parameter)
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>($"Prefabs/LevelPrefabs/level {parameter}");

        resourceRequest.completed += operation =>
        {
            GameObject newLevel = Object.Instantiate(resourceRequest.asset.GameObject(), Vector3.zero, Quaternion.identity);
            if (newLevel != null)
            {
                newLevel.transform.SetParent(_levelManager.levelHolder.transform);
            }
        };
    }
}
