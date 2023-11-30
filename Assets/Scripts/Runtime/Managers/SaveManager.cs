using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static GameData _gameData;

    public void SaveData()
    {
        ES3.Save<byte>("Level", _gameData.Level);
    }

    public static byte GetSavedData()
    {
        byte level = ES3.Load<byte>("Level");
        _gameData.Level = level;

        return _gameData.Level;
    }


    private void OnApplicationQuit()
    {
        SaveData();
    }

}
