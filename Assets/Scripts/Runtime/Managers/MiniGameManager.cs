using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject wallObjects;
    [SerializeField]
    private GameObject fakeMoneyObject;
    [SerializeField]
    private Transform fakePlayer;
    [SerializeField]
    private Material mat;
    [SerializeField]
    private short wallCount;
    [SerializeField]
    private short fakeMoneyCount;

    [SerializeField]
    private WallCheckController wallChecker;

    private int _score;
    private float _multiplier;
    private Vector3 _initializePos;

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        ScoreSignals.Instance.onSendFinalScore += OnSendScore;
        ScoreSignals.Instance.onGetMultiplier += OnGetMultiplier;
        CoreGameSignals.Instance.onMiniGameStart += OnMiniGameStart;
        CoreGameSignals.Instance.onReset += OnReset;
    }

    private void OnMiniGameStart()
    {
        fakePlayer.gameObject.SetActive(true);
        StartCoroutine(GoUp());
    }

    private IEnumerator GoUp()
    {
        //1 saniye sonra iþlemlerin baþlatýlmasýnýn sebebi kamera deðiþikliði yaþanýyor o yüzden.
        yield return new WaitForSeconds(1f);
        if (_score == 0)
        {
            CoreGameSignals.Instance.onLevelFailed?.Invoke();
        }
        else
        {
            //Burada score deðerimize göre 0 ila 900 deðeri arasýnda(Eðer score deðeri 0'dan düþükse 0'a 900'den yüksekse 900'e eþitleniyor.
            //Mathf.Clamp sayesinde) 2.7 saniyede yükseliþ gerçekleþiyor, 1 saniye bekleyip sonra
            //4.5 saniye daha bekleyip sinyal tetikleniyor.
            fakePlayer.DOLocalMoveY(Mathf.Clamp(_score, 0, 900), 2.7f).SetEase(Ease.Flash).SetDelay(1f);
            yield return new WaitForSeconds(4.5f);
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
        }
    }

    internal void SetMultiplier(float multiplierValue)
    {
        _multiplier = multiplierValue;
    }

    private float OnGetMultiplier()
    {
        return _multiplier;
    }

    private void OnSendScore(int scoreValue)
    {
        _score = scoreValue;
    }

    private void UnSubscribeEvents()
    {
        ScoreSignals.Instance.onSendFinalScore -= OnSendScore;
        ScoreSignals.Instance.onGetMultiplier -= OnGetMultiplier;
        CoreGameSignals.Instance.onMiniGameStart -= OnMiniGameStart;
        CoreGameSignals.Instance.onReset -= OnReset;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void Start()
    {
        SpawnWallObjects();
        SpawnFakeMoneyObjects();
        Init();
    }

    private void Init()
    {
        _initializePos = fakePlayer.localPosition;
    }

    private void SpawnWallObjects()
    {
        for (int i = 0; i < wallCount; i++)
        {
            GameObject wall = Instantiate(wallObjects,transform);
            wall.transform.localPosition = new Vector3(0, i + 10, 0);
            wall.transform.GetChild(0).GetComponent<TextMeshPro>().text = "x" + ((i / 10f) + 1f);
        }
    }


    private void SpawnFakeMoneyObjects()
    {
        for (int i = 0; i < fakeMoneyCount; i++)
        {
            GameObject fakeMoneyGo = Instantiate(fakeMoneyObject, fakePlayer);
            fakeMoneyGo.transform.localPosition = new Vector3(0, -i * 1.58f, -7);
        }
    }

    //Burada duvarlar tekrar tekrar instantiate edilmemesi için tüm duvarlar tekrardan eski renklerine döndürülüp eski
    //konumlarýna getiriliyor.
    private void ResetWalls()
    {
        for (int i = 0; i < wallCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = mat;
            transform.GetChild(i).position = Vector3.zero;
        }
    }
    
    private void ResetFakePlayer()
    {
        fakePlayer.gameObject.SetActive(false);
        fakePlayer.transform.localPosition = _initializePos;
    }

    private void OnReset()
    {
        //Moneylerin öne doðru itilmesi, ekrana gelmesi, Playerýn yukarý doðru yükselmesi için Courutine kullanýyoruz ve durduruyoruz burada.
        StopAllCoroutines();
        //Player yukarý doðru çýkmasý Dotween ile saðlanýyor, dotweenler durduruluyor.
        DOTween.KillAll();
        ResetWalls();
        ResetFakePlayer();
        //Buraya neden OnReset dedik Observer Pattern'e girdiðin için mi sor.
        wallChecker.OnReset();
    }
}
