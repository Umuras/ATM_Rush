using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fiziksel etkile�imlerde Player dedi�imiz yani hareketli olan hareket alan aksiyon alan kimse fiziksel etkile�imi yapan odur.
//Dolay�s�yla etkile�imle alakal� b�t�n i�lemleri onda tutmam�z laz�m. Duvarlara de�en Player�n kendisi oldu�u i�in bu scriptin
//Player �zerinde olmas� laz�m.
public class WallCheckController : MonoBehaviour
{
    [SerializeField]
    private MiniGameManager manager;

    private float _changesColor;
    private float _multiplier;

    private readonly string _wall = "Wall";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_wall))
        {
            return;
        }
        _multiplier += 0.1f;
        manager.SetMultiplier(_multiplier);
        ChangeColor(other);
    }

    private void ChangeColor(Collider other)
    {
        //0 ile 1 aras�nda de�er alabilmek i�in 1'e g�re modu al�n�yor.
        _changesColor = (0.036f + _changesColor) % 1;
        GameObject otherGameObject = other.gameObject;
        otherGameObject.GetComponent<Renderer>().material.DOColor(Color.HSVToRGB(_changesColor, 1, 1), 0.1f);
        otherGameObject.transform.DOLocalMoveZ(-3, 0);
    }

    internal void OnReset()
    {
        _changesColor = 0;
        _multiplier = 0.90f;
    }
}
