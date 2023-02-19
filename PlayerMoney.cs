using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoney
{
    public static int Currency { get { return PlayerPrefs.GetInt("currency", 0); } set { PlayerPrefs.SetInt("currency", value); } }
    public static void AddCurrency(int amount) => Currency += amount;
    public static bool TryBuySkin(int price) => Currency >= price;
    public static void DecreaseCurrency(int amount) => Currency -= amount;
    public static int GetCurrency() => Currency;
}
