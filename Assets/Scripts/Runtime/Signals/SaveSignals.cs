using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSignals : MonoSingleton<SaveSignals>
{
    public UnityAction onSaveGameData = delegate { };
}
