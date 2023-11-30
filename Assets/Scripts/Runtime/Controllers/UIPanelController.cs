using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> layers = new List<GameObject>();

    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
        CoreUISignals.Instance.onClosePanel += OnClosePanel;
        CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanels;
    }

    private void OnOpenPanel(UIPanelTypes panel, byte layerValue)
    {
        CoreUISignals.Instance.onClosePanel?.Invoke(layerValue);
        Instantiate(Resources.Load<GameObject>($"Screens/{panel}Panel"), layers[layerValue].transform);
    }

    private void OnClosePanel(byte layerValue)
    {
        if (layers[layerValue].transform.childCount > 0)
        {
            for (int i = 0; i < layers[layerValue].transform.childCount; i++)
            {
                Destroy(layers[layerValue].transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnCloseAllPanels()
    {
        for (int i = 0; i < layers.Count; i++)
        {
            if (layers[i].transform.childCount <= 0)
            {
                continue;
            }

            for (int j = 0; j < layers[i].transform.childCount; j++)
            {
                Destroy(layers[i].transform.GetChild(j).gameObject);
            }
        }
    }
}
