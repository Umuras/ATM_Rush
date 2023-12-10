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
        //Burada toplanabilir obje gate kýsmýndan geçtiðinde gold mu yoksa diamonda mý yükseleceði belirleniyor.
        //money 0'a, gold 1'e, diamond'da 2'ye eþit.
        if (_currentValue < 2)
        {
            _currentValue++;
        }
        //Burada mesh deðiþimi yapýyoruz.
        collectableMeshController.OnUpgradeCollectableVisual(_currentValue);
        //Total skoru güncelliyor.
        StackSignals.Instance.onUpdateType?.Invoke();
    }

    public byte GetCurrentValue()
    {
        return _currentValue;
    }

    //Temas ettiðimiz toplanabilir objeyi listeye eklemek için kullanýyoruz.
    public void InteractionWithCollectable(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionCollectable?.Invoke(collectableGameObject);
    }

    //ATM'ye temas ettiðimizde toplanabilir objeyi stack listesinden çýkarýp para olarak geçmesi için kullanýyoruz.
    //ScoreManager'daki money tutucuya veri göndermek için kullanýyoruz.
    public void InteractionWithAtm(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionATM?.Invoke(collectableGameObject);
    }

    //Obstacle ile etkileþime geçince çalýþacak.
    public void InteractionWithObstacle(GameObject collectableGameObject)
    {
        StackSignals.Instance.onInteractionObstacle?.Invoke(collectableGameObject);
    }

    //Toplanabilir objelerin Conveyor ile etkileþime geçince çalýþacak.
    public void InteractionWithConveyor()
    {
        StackSignals.Instance.onInteractionConveyor?.Invoke();
    }
}
