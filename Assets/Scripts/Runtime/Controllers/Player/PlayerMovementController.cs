using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private PlayerManager manager;
    [SerializeField]
    private new Rigidbody rigidbody;

    private PlayerMovementData _data;

    [Header("Additional Value")]
    private bool _isReadyToPlay;
    [Header("Additional Value")]
    private bool _isReadyToMove;

    private float _inputValue;
    private Vector2 _clampValues;

    internal void IsReadyToMove(bool condition)
    {
        _isReadyToMove = condition;
    }

    internal void IsReadyToPlay(bool condition)
    {
        _isReadyToPlay = condition;
    }
    //PlayerManager �zerinde movementData g�nderilip scriptableObject �zerindeki movementData verilerinin kullanabilmesi i�in olu�turuldu.
    internal void SetMovementData(PlayerMovementData movementData)
    {
        _data = movementData;
    }
    //InputDragged �al��t���nda(InputManagerda �al���yor) o anki g�ncel verileri almak i�in olu�turuldu.
    internal void UpdateInputValue(HorizontalInputParams inputParams)
    {
        _inputValue = inputParams.HorizontalInputValue;
        _clampValues = inputParams.HorizontalInputClampSides;
    }
    //Karakter oynanabilir haldeyken toplanabilir objelerin posizyonu ayarlan�yor, s�rekli olarak. 
    private void Update()
    {
        if (_isReadyToPlay)
        {
            manager.SetStackPosition();
        }
    }

    private void FixedUpdate()
    {
        if (_isReadyToPlay)
        {
            if (_isReadyToMove)
            {
                Move();
            }
            else
            {
                StopSideways();
            }
        }
        else
        {
            Stop();
        }
    }
    //Karakterin �ne do�ru gitti�i sabit h�z� ve sa�a sola gitme h�z� ve sa�a sola gitme s�n�r� ayarlan�yor.
    private void Move()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity = new Vector3(_inputValue * _data.SidewaysSpeed, velocity.y, _data.ForwardSpeed);
        rigidbody.velocity = velocity;
        //Karakterin pozisyonunu _clampValues -5 ve 5 de�erince k�s�tl�yor.
        Vector3 position = rigidbody.position;
        position = new Vector3(Math.Clamp(position.x, _clampValues.x, _clampValues.y),(position = rigidbody.position).y,position.z);
        rigidbody.position = position;
    }

    //Telefondan parma��m�z� �ekince �al��acak k�s�m. Sa�a sola gidi� engelleniyor.
    private void StopSideways()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
        rigidbody.angularVelocity = Vector3.zero;
    }
    //Karakter komple duruyor.
    private void Stop()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    internal void OnReset()
    {
        Stop();
        _isReadyToMove = false;
        _isReadyToPlay = false;
    }
}
