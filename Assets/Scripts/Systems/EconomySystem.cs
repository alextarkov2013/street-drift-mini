using UnityEngine;
using System;

public class EconomySystem : MonoBehaviour
{
    public static EconomySystem Instance { get; private set; }

    private float totalMoney = 1000f; // Starting money
    public Action<float> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddMoney(float amount)
    {
        totalMoney += amount;
        OnMoneyChanged?.Invoke(totalMoney);
    }

    public void RemoveMoney(float amount)
    {
        if (totalMoney >= amount)
        {
            totalMoney -= amount;
            OnMoneyChanged?.Invoke(totalMoney);
            return;
        }
        Debug.LogWarning("Not enough money!");
    }

    public bool CanAfford(float amount)
    {
        return totalMoney >= amount;
    }

    public float GetTotalMoney() => totalMoney;

    public void SetMoney(float amount)
    {
        totalMoney = amount;
        OnMoneyChanged?.Invoke(totalMoney);
    }
}
