using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;

    //Karakter para toplad���nda karakterin �zerindeki scoretexti g�ncellemesi i�in olu�turuldu.
    internal void OnSetTotalScore(int value)
    {
        scoreText.text = value.ToString();
    }

}
