using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    [SerializeField]
    private CollectableMeshController collectableMeshController;
    [SerializeField]
    private CollectablePhysicsController collectablePhysicsController;


    private CollectableData _data;
    private byte _currentValue;

    private readonly string _collectableDataPath = "Data/CD_Collectable";

    private void Awake()
    {
        _data = GetCollectableData();
        SendDataToController();
    }

    private CollectableData GetCollectableData()
    {
        return Resources.Load<CD_Collectable>(_collectableDataPath).Data;
    }
    private void SendDataToController()
    {
        collectableMeshController.SetMeshData(_data.MeshData);
    }


    internal void CollectableUpgrade(int value)
    {
        //Burada toplanabilir obje gate k�sm�ndan ge�ti�inde gold mu yoksa diamonda m� y�kselece�i belirleniyor.
        //money 0'a, gold 1'e, diamond'da 2'ye e�it.
        if (_currentValue < 2)
        {
            _currentValue++;
        }
        //Burada mesh de�i�imi yap�yoruz.
        collectableMeshController.OnUpgradeCollectableVisual(_currentValue);
        //Total skoru g�ncelliyor.
        StackSignals.Instance.onUpdateType?.Invoke();
    }

    public byte GetCurrentValue()
    {
        return _currentValue;
    }

    //Temas etti�imiz toplanabilir objeyi listeye eklemek i�in kullan�yoruz.
    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
    }

    //ATM'ye temas etti�imizde toplanabilir objeyi stack listesinden ��kar�p para olarak ge�mesi i�in kullan�yoruz.
    //ScoreManager'daki money tutucuya veri g�ndermek i�in kullan�yoruz.
    public void InteractionWithAtm(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionATM?.Invoke(collectableGameObject);
    }

    //Obstacle ile etkile�ime ge�ince �al��acak.
    public void InteractionWithObstacle(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
    }

    //Toplanabilir objelerin Conveyor ile etkile�ime ge�ince �al��acak.
    public void InteractionWithConveyor()
    {
        StackSignals.Instance.onInteractionConveyor?.Invoke();
    }
}
