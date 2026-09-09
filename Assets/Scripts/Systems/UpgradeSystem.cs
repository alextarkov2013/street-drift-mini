using UnityEngine;
using System;
using System.Collections.Generic;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance { get; private set; }

    [System.Serializable]
    public class UpgradeData
    {
        public string upgradeName = "ENGINE";
        public int currentLevel = 0;
        public float nextUpgradeCost = 1000f;
    }

    private Dictionary<string, UpgradeData> upgrades = new Dictionary<string, UpgradeData>();
    public Action<string> OnUpgradeApplied;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        string[] upgradeTypes = { "ENGINE", "TURBO", "TIRES", "BRAKES", "SUSPENSION" };
        foreach (string upgradeType in upgradeTypes)
        {
            upgrades[upgradeType] = new UpgradeData
            {
                upgradeName = upgradeType,
                currentLevel = 0,
                nextUpgradeCost = 1000f
            };
        }
    }

    public bool TryUpgrade(string upgradeName, PlayerCar car)
    {
        if (!upgrades.ContainsKey(upgradeName)) return false;

        UpgradeData upgrade = upgrades[upgradeName];
        if (upgrade.currentLevel >= 5) return false;

        if (EconomySystem.Instance.CanAfford(upgrade.nextUpgradeCost))
        {
            EconomySystem.Instance.RemoveMoney(upgrade.nextUpgradeCost);
            upgrade.currentLevel++;
            car.ApplyUpgrade(upgradeName, upgrade.currentLevel);
            
            // Calculate next cost
            upgrade.nextUpgradeCost = 1000f * upgrade.currentLevel * 1.5f;
            
            OnUpgradeApplied?.Invoke(upgradeName);
            return true;
        }

        return false;
    }

    public UpgradeData GetUpgrade(string upgradeName)
    {
        return upgrades.ContainsKey(upgradeName) ? upgrades[upgradeName] : null;
    }

    public Dictionary<string, UpgradeData> GetAllUpgrades() => upgrades;
}
