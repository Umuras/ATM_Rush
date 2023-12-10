using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;

    internal void OnSetTotalScore(int value)
    {
        scoreText.text = value.ToString();
    }

}
