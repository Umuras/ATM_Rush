using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreSignals : MonoSingleton<ScoreSignals>
{
    public UnityAction<int> onSetScore = delegate { };
    public UnityAction<int> onSetAtmScore = delegate { };
    //public UnityAction<int> onSetAtmScoreText = delegate { };
    public UnityAction<int> onSendFinalScore = delegate { };
    public UnityAction<int> onSendMoney = delegate { };
    public Func<int> onGetMoney = delegate { return 0; };

    // Bir sonraki seviye ba�lamadan evvelki alaca��m�z toplam paray� belirliyor.
    //Para taraf�n� UI �zerinden g�nderiyoruz, ��nk� paray� UI'da yazd�r�yoruz.
    public Func<float> onGetMultiplier = delegate { return 0f; };
}
