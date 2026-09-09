using UnityEngine;

[System.Serializable]
public class UpgradeLevel
{
    public int level = 1;
    public float cost = 1000f;
    public float statIncrease = 10f;
}

[System.Serializable]
public class UpgradeConfig
{
    public string upgradeName = "ENGINE";
    public UpgradeLevel[] levels = new UpgradeLevel[5];
}

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Street Drift Mini/Upgrade Config")]
public class UpgradeConfigAsset : ScriptableObject
{
    public UpgradeConfig[] upgrades = new UpgradeConfig[5];
}
