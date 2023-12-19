using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _money;
    private int _stackValueMultiplier;
    private int _scoreCache = 0;
    private int _atmScoreValue = 0;

    private void Awake()
    {
        _money = GetMoneyValue();
    }

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        ScoreSignals.Instance.onSendMoney += OnSendMoney;
        ScoreSignals.Instance.onGetMoney += OnGetMoney;
        ScoreSignals.Instance.onSetScore += OnSetScore;
        ScoreSignals.Instance.onSetAtmScore += OnSetAtmScore;
        CoreGameSignals.Instance.onMiniGameStart += OnSendFinalScore;
        UISignals.Instance.onClickIncome += OnSetValueMultiplier;
        CoreGameSignals.Instance.onReset += OnReset;
        CoreGameSignals.Instance.onLevelSuccessful += RefreshMoney;
        CoreGameSignals.Instance.onLevelFailed += RefreshMoney;

    }

    private void OnSendMoney(int value)
    {
        _money = value;
    }

    private int OnGetMoney()
    {
        return _money;
    }
    //Karakter her para topladýðýnda StackTypeUpdaterCommand üzerinde hesaplanan totalListScore deðeri gönderilip karakter üzerindeki 
    //totalScore yazdýrýlýr.
    private void OnSetScore(int setScore)
    {
        //Burada ise setScore StackTypeUpdaterCommand üzerinde hesaplanan totalListScoredýr, _stackValueMultiplier(IncomeLeveldýr), _atmScoreValue
        //ise OnSetAtmScore üzerinde hesaplanýr. totalListScore toplanan objenin + 1 deðeri üzerinden gelmektedir ve bu fonksiyon her obje için çalýþtýðý için
        //_scoreCache deðeri ve setScore deðiþmektedir.
        _scoreCache = (setScore * _stackValueMultiplier) + _atmScoreValue;
        //Burada da totalListScoreu Playerýn üstündeki score textine yazdýrýyoruz.
        PlayerSignals.Instance.onSetTotalScore?.Invoke(setScore);
    }
    //_atmScoreValue deðeri kendisi ile toplanýp atmye deðen toplanabilir objenin bir fazlasý üzerinden(atmValues) ve _stackValueMultiplier yani IncomeLevel ile çarpýlarak hesaplanýyor
    //ardýndan texti üzerine yazdýrýlýyor.
    private void OnSetAtmScore(int atmValues)
    {
        _atmScoreValue += atmValues * _stackValueMultiplier;
        AtmSignals.Instance.onSetAtmScoreText?.Invoke(_atmScoreValue);
    }

    private void OnSendFinalScore()
    {
        ScoreSignals.Instance.onSendFinalScore?.Invoke(_scoreCache);
    }

    private void OnSetValueMultiplier()
    {
        _stackValueMultiplier = (int)CoreGameSignals.Instance.onGetIncomeLevel?.Invoke();
    }

    private void OnReset()
    {
        //Çarpan olduðu için 0lanmasý gerekiyor, kümülatif etki etmesini istemiyoruz.
        _scoreCache = 0;
        _atmScoreValue = 0;
    }

    private void UnSubscribeEvents()
    {
        ScoreSignals.Instance.onSendMoney -= OnSendMoney;
        ScoreSignals.Instance.onGetMoney -= OnGetMoney;
        ScoreSignals.Instance.onSetScore -= OnSetScore;
        ScoreSignals.Instance.onSetAtmScore -= OnSetAtmScore;
        CoreGameSignals.Instance.onMiniGameStart -= OnSendFinalScore;
        UISignals.Instance.onClickIncome -= OnSetValueMultiplier;
        CoreGameSignals.Instance.onReset -= OnReset;
        CoreGameSignals.Instance.onLevelSuccessful -= RefreshMoney;
        CoreGameSignals.Instance.onLevelFailed -= RefreshMoney;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Start()
    {
        OnSetValueMultiplier();
        RefreshMoney();
    }

    private int GetMoneyValue()
    {
        if (!ES3.FileExists())
        {
            return 0;
        }
        else
        {
            return (int)(ES3.KeyExists("Money") ? ES3.Load<int>("Money") : 0);
        }
    }
    //OnGetMultiplier minigame esnasýnda her deðdiði kare oranýnda 0.1 artarak belirlenen deðer oluyor.
    //scoreCache ise OnSetMoneyde hesaplanmaktadýr, RefreshMoney, MiniGame bittikten sonra yani oyun sonunda ve oyun baþýnda hesaplanmaktadýr.
    private void RefreshMoney()
    {
        _money += (int)(_scoreCache * ScoreSignals.Instance.onGetMultiplier?.Invoke());
        UISignals.Instance.onSetMoneyValue?.Invoke(_money);
    }
}
