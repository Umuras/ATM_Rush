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

    internal void SetMovementData(PlayerMovementData movementData)
    {
        _data = movementData;
    }

    internal void UpdateInputValue(HorizontalInputParams inputParams)
    {
        _inputValue = inputParams.HorizontalInputValue;
        _clampValues = inputParams.HorizontalInputClampSides;
    }

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

    private void Move()
    {
        Vector3 velocity = rigidbody.velocity;
        velocity = new Vector3(_inputValue * _data.SidewaysSpeed, velocity.y, _data.ForwardSpeed);
        rigidbody.velocity = velocity;

        Vector3 position = rigidbody.position;
        position = new Vector3(Math.Clamp(position.x, _clampValues.x, _clampValues.y),(position = rigidbody.position).y,position.z);
        rigidbody.position = position;
    }

    //Telefondan parmaðýmýzý çekince çalýþacak kýsým.
    private void StopSideways()
    {
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, _data.ForwardSpeed);
        rigidbody.angularVelocity = Vector3.zero;
    }

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
