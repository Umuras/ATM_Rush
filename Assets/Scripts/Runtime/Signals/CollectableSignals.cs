using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectableSignals : MonoSingleton<CollectableSignals>
{
    public UnityAction<int> onCollectableUpgrade = delegate { };
}
