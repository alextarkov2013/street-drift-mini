using UnityEngine;

[System.Serializable]
public class CarConfig
{
    public string carName = "Starter";
    public string carDescription = "";
    public float price = 0f;
    public bool isStarter = false;

    [System.Serializable]
    public class Stats
    {
        public float power = 100f;
        public float maxSpeed = 40f;
        public float handling = 0.5f;
        public float driftability = 0.5f;
        public float braking = 80f;
    }

    public Stats baseStats = new Stats();
}

[CreateAssetMenu(fileName = "CarConfig", menuName = "Street Drift Mini/Car Config")]
public class CarConfigAsset : ScriptableObject
{
    public CarConfig config = new CarConfig();
}
