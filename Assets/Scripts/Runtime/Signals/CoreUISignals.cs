using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoreUISignals : MonoSingleton<CoreUISignals>
{
    //public static CoreUISignals Instance;

    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;
    //}

    public UnityAction onCloseAllPanels = delegate { };
    public UnityAction<UIPanelTypes, byte> onOpenPanel = delegate { };
    public UnityAction<byte> onClosePanel = delegate { };
}
