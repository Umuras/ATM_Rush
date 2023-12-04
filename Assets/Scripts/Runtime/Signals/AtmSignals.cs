using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AtmSignals : MonoSingleton<AtmSignals>
{
    public UnityAction<int> onSetAtmScoreText = delegate { };
    
}
