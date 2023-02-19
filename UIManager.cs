using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text text;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayerMoney.AddCurrency(200);
        }
        text.text = PlayerMoney.Currency.ToString();
    }

    public void Clear()
    {
        PlayerPrefs.DeleteAll();
    }
}
