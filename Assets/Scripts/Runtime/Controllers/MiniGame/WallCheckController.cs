using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Fiziksel etkileþimlerde Player dediðimiz yani hareketli olan hareket alan aksiyon alan kimse fiziksel etkileþimi yapan odur.
//Dolayýsýyla etkileþimle alakalý bütün iþlemleri onda tutmamýz lazým. Duvarlara deðen Playerýn kendisi olduðu için bu scriptin
//Player üzerinde olmasý lazým.
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
        //0 ile 1 arasýnda deðer alabilmek için 1'e göre modu alýnýyor.
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
