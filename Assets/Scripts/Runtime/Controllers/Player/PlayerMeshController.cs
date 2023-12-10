using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        PlayerSignals.Instance.onSetTotalScore += OnSetTotalScore;
    }

    public void OnSetTotalScore(int value)
    {
        scoreText.text = value.ToString();
    }

    private void UnSubscribeEvents()
    {
        PlayerSignals.Instance.onSetTotalScore -= OnSetTotalScore;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
}
