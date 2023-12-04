using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtmManager : MonoBehaviour
{
    [SerializeField]
    private DOTweenAnimation doTweenAnimation;
    [SerializeField]
    private TextMeshPro atmText;

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        doTweenAnimation = GetComponentInChildren<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onAtmTouched += OnAtmTouched;
        AtmSignals.Instance.onSetAtmScoreText += OnSetAtmScoreText;
    }

    private void OnAtmTouched(GameObject touchedATMObject)
    {
        if (touchedATMObject.GetInstanceID() == gameObject.GetInstanceID())
        {
            doTweenAnimation.DOPlay();
        }
    }

    private void OnSetAtmScoreText(int value)
    {
        atmText.text = value.ToString();
    }

    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onAtmTouched -= OnAtmTouched;
        AtmSignals.Instance.onSetAtmScoreText -= OnSetAtmScoreText;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
