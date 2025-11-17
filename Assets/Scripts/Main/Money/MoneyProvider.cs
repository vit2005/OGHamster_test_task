using System;
using UnityEngine;

public class MoneyProvider
{
    private int _amount;
    public int Amount => _amount; 
    public static event Action<int> OnMoneyChanged;

    public void Init()
    {
        if (!PlayerPrefs.HasKey("Money"))
            PlayerPrefs.SetInt("Money", 0);

        _amount = PlayerPrefs.GetInt("Money");
    }

    public void IncreaseMoney(int amount = 1)
    {
        _amount += amount;
        OnMoneyChanged?.Invoke(_amount);
    }

    public void DecreaseMoney(int amount = 1)
    {
        _amount -= amount;
        if (_amount < 0)
        {
            _amount = 0;
            Debug.LogWarning("Money cannot be negative. Setting to 0.");
        }
            
        OnMoneyChanged?.Invoke(_amount);
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", _amount);
        PlayerPrefs.Save();
    }
}
