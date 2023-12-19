using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro scoreText;

    //Karakter para topladığında karakterin üzerindeki scoretexti güncellemesi için oluşturuldu.
    internal void OnSetTotalScore(int value)
    {
        scoreText.text = value.ToString();
    }

}
