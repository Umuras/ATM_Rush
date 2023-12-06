using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackJumperCommand
{
    private StackData _data;
    private List<GameObject> _collectableStack;
    private Transform _levelHolder;
    public StackJumperCommand(StackData stackData, ref List<GameObject> collectableStack)
    {
        _data = stackData;
        _collectableStack = collectableStack;
        _levelHolder = GameObject.Find("LevelHolder").transform;
    }

    public void Execute(int lastIndex, int index)
    {
        for (int i = lastIndex; i > index; i--)
        {
            _collectableStack[i].transform.GetChild(1).tag = "Collectable";
            _collectableStack[i].transform.SetParent(_levelHolder.transform.GetChild(0));
            //-_data.JumpItemsClampX, _data.JumpItemsClampX + 1 x ekseninde collectableGameObjectlerin f�rlat�laca�� alan belirleniyor.
            //1.12f y ekseninde o y�ksekli�e f�rlat�l�yor, z ekseninde normal konumunun 10-15 birim aras� �n�ne f�rlat�l�yor, 
            //_data.JumpForceda at�lma g�c� belirleniyor, numJumpsda [Random.Range(1,3)] ka� iterasyonda yapaca�� belirleniyor, 0.5f s�rede
            //yap�l�yor.
            _collectableStack[i].transform.DOJump(new Vector3(Random.Range(-_data.JumpItemsClampX, _data.JumpItemsClampX + 1), 1.12f,
                _collectableStack[i].transform.position.z + Random.Range(10, 15)), _data.JumpForce, Random.Range(1, 3), 0.5f);
            //DOPunchScale relative bir ekleme yap�yor, yani var olan scale de�erinin �zerine ekliyor.
            _collectableStack[i].transform.DOScale(Vector3.one, 0);
            _collectableStack.RemoveAt(i);
            _collectableStack.TrimExcess();
        }
    }
}
