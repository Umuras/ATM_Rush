using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI incomeLvlText;
    [SerializeField]
    private Button incomeLvlButton;
    [SerializeField]
    private TextMeshProUGUI incomeValue;
    [SerializeField]
    private Button stackLvlButton;
    [SerializeField]
    private TextMeshProUGUI stackLvlText;
    [SerializeField]
    private TextMeshProUGUI stackValue;


    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        UISignals.Instance.onSetIncomeLvlText += OnSetIncomeLvlText;
        UISignals.Instance.onSetStackLvlText += OnSetStackLvlText;
    }

    private void OnSetStackLvlText()
    {
        stackLvlText.text = "Stack lvl\n" + CoreGameSignals.Instance.onGetStackLevel?.Invoke();
        //Burada 2'nin üssünü alýp 100 ile çarpýyoruz. Clamp fonksiyonu ise GetStackLevel deðerini 0 ile 10 arasýnda sýnýrlýyor, 0'dan küçük gelirse
        //0 oluyor, 10'dan büyük gelirse 10 oluyor. Örn. 2^3 = 8 *100 = 800 geliyor mesela.
        stackValue.text = (Mathf.Pow(2, Mathf.Clamp((byte)CoreGameSignals.Instance.onGetStackLevel?.Invoke(), (byte)0, (byte)10)) * 100).
            ToString();
    }

    private void OnSetIncomeLvlText()
    {
        incomeLvlText.text = "Income lvl\n" + CoreGameSignals.Instance.onGetIncomeLevel?.Invoke();
        incomeValue.text = (Mathf.Pow(2, Mathf.Clamp((byte)CoreGameSignals.Instance.onGetIncomeLevel ?.Invoke(), (byte)0, (byte)10)) * 100).
            ToString();
    }

    private void UnSubscribeEvents()
    {
        UISignals.Instance.onSetIncomeLvlText -= OnSetIncomeLvlText;
        UISignals.Instance.onSetStackLvlText -= OnSetStackLvlText;
    }

    private void OnDisable()
    {
        UnSubscribeEvents();
    }

    private void Start()
    {
        SyncShopUi();
    }

    private void SyncShopUi()
    {
        OnSetIncomeLvlText();
        OnSetStackLvlText();
        ChangesIncomeInteractable();
        ChangesStackInteractable();
    }

    private void ChangesIncomeInteractable()
    {
        //Bu þekilde ToString()! ifadesi sinyalden gelecek olan ifadenin kesinlikle null olmayacaðýnýn garantisini verir.
        if (int.Parse(UISignals.Instance.onGetMoneyValue?.Invoke().ToString()!) < int.Parse(incomeValue.text) || CoreGameSignals.Instance.onGetIncomeLevel?.Invoke() >= 30)
        {
            incomeLvlButton.interactable = false;
        }
        else
        {
            incomeLvlButton.interactable = true;
        }
    }


    private void ChangesStackInteractable()
    {
        if (UISignals.Instance.onGetMoneyValue?.Invoke() < int.Parse(stackValue.text) || CoreGameSignals.Instance.onGetStackLevel?.Invoke() >= 15)
        {
            stackLvlButton.interactable = false;
        }
        else
        {
            stackLvlButton.interactable = true;
        }
    }

}
