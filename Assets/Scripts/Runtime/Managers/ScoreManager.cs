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
    //Income veya Stack butonuna t�klay�nca elde edilen yeni paray� y�kl�yoruz.
    private void OnSendMoney(int value)
    {
        _money = value;
    }

    private int OnGetMoney()
    {
        return _money;
    }
    //Karakter her para toplad���nda StackTypeUpdaterCommand �zerinde hesaplanan totalListScore de�eri g�nderilip karakter �zerindeki 
    //totalScore yazd�r�l�r.
    private void OnSetScore(int setScore)
    {
        //Burada ise setScore StackTypeUpdaterCommand �zerinde hesaplanan totalListScored�r, _stackValueMultiplier(IncomeLeveld�r), _atmScoreValue
        //ise OnSetAtmScore �zerinde hesaplan�r. totalListScore toplanan objenin + 1 de�eri �zerinden gelmektedir ve bu fonksiyon her obje i�in �al��t��� i�in
        //_scoreCache de�eri ve setScore de�i�mektedir.
        //Burada _atmScoreValue ile toplamas�n�n sebebi paray� atmye yat�rd���nda atmde olan paray� da ekleyip �yle _scoreCache de�eri hesaplan�yor.
        //_scoreCache oyunun sonunda toplanan t�m para oluyor asl�nda
        _scoreCache = (setScore * _stackValueMultiplier) + _atmScoreValue;
        Debug.LogError($"scoreCache {_scoreCache}, stackValue(IncomeLevel) = {_stackValueMultiplier}, atmScore = {_atmScoreValue}");
        //Burada da totalListScoreu Player�n �st�ndeki score textine yazd�r�yoruz.
        PlayerSignals.Instance.onSetTotalScore?.Invoke(setScore);
    }
    //_atmScoreValue de�eri kendisi ile toplan�p atmye de�en toplanabilir objenin bir fazlas� �zerinden(atmValues) ve _stackValueMultiplier yani IncomeLevel ile �arp�larak hesaplan�yor
    //ard�ndan texti �zerine yazd�r�l�yor.
    private void OnSetAtmScore(int atmValues)
    {
        _atmScoreValue += atmValues * _stackValueMultiplier;
        Debug.LogError($"atmScoreValue = {_atmScoreValue} atmValues = {atmValues} stackValueMultiplies(IncomeLevel) = {_stackValueMultiplier}");
        AtmSignals.Instance.onSetAtmScoreText?.Invoke(_atmScoreValue);
    }
    //Oyundaki son score de�erini g�ndererek minigamede karakterin ne kadar y�kselece�i belli oluyor.
    private void OnSendFinalScore()
    {
        ScoreSignals.Instance.onSendFinalScore?.Invoke(_scoreCache);
    }
    //IncomeLevel� getiriyoruz ve stackValueMultipliere y�kl�yoruz.
    private void OnSetValueMultiplier()
    {
        _stackValueMultiplier = (int)CoreGameSignals.Instance.onGetIncomeLevel?.Invoke();
    }

    private void OnReset()
    {
        //�arpan oldu�u i�in 0lanmas� gerekiyor, k�m�latif etki etmesini istemiyoruz.
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
    //OnGetMultiplier minigame esnas�nda her de�di�i kare oran�nda 0.1 artarak belirlenen de�er oluyor.
    //scoreCache ise OnSetMoneyde hesaplanmaktad�r, RefreshMoney, MiniGame bittikten sonra yani oyun sonunda ve oyun ba��nda hesaplanmaktad�r.
    private void RefreshMoney()
    {
        _money += (int)(_scoreCache * ScoreSignals.Instance.onGetMultiplier?.Invoke());
        UISignals.Instance.onSetMoneyValue?.Invoke(_money);
    }
}
