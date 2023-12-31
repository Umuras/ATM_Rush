using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [Header("Data")]
    private InputData _data;
    
    [Header("Additional Variables")]
    private bool _isAvailableForTouch;

    private bool _isFirstTimeTouchTaken;

    private float _positionValuesX;
    private bool _isTouching;

    private float _currentVelocity;
    private Vector2? _mousePosition;
    private Vector3 _moveVector;

    private void Awake()
    {
        _data = GetInputData();   
    }

    private InputData GetInputData()
    {
        return Resources.Load<CD_Input>("Data/CD_Input").Data;
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onReset += OnReset;
        CoreGameSignals.Instance.onPlay += OnPlay;
        InputSignals.Instance.onChangeInputState += OnChangeInputState;
    }

    private void OnReset()
    {
        _isTouching = false;
        _isFirstTimeTouchTaken = false;
    }

    private void OnChangeInputState(bool state)
    {
        _isAvailableForTouch = state;
    }

    private void OnPlay()
    {
        _isAvailableForTouch = true;
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onReset -= OnReset;
        CoreGameSignals.Instance.onPlay -= OnPlay;
        InputSignals.Instance.onChangeInputState -= OnChangeInputState;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Update()
    {
        if (!_isAvailableForTouch)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
        {
            _isTouching = false;

            InputSignals.Instance.onInputReleased?.Invoke();
        }

        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            _isTouching = true;

            InputSignals.Instance.onInputTaken?.Invoke();
            if (!_isFirstTimeTouchTaken)
            {
                _isFirstTimeTouchTaken = true;
                InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
            }

            _mousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
        {
            if (_isTouching)
            {
                if (_mousePosition != null)
                {
                    Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;

                    if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                    {
                        _moveVector.x = _data.HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                    }
                    else if (mouseDeltaPos.x < -_data.HorizontalInputSpeed)
                    {
                        _moveVector.x = -_data.HorizontalInputSpeed / 10f * -mouseDeltaPos.x;
                    }
                    else
                    {
                        _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity,
                            _data.HorizontalInputClampStopValue);
                    }

                    _mousePosition = Input.mousePosition;

                    InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                    {
                        HorizontalInputValue = _moveVector.x,
                        HorizontalInputClampSides = _data.HorizontalInputClampSides
                    });
                }
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
