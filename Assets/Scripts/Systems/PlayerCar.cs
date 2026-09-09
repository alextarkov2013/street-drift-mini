using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    [System.Serializable]
    public class CarStats
    {
        public float power = 100f;
        public float maxSpeed = 50f;
        public float handling = 0.5f;
        public float driftability = 0.6f;
        public float braking = 100f;
    }

    public CarStats stats = new CarStats();
    public string carType = "Starter";
    public Color carColor = Color.white;

    private Rigidbody rb;
    private CarController controller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CarController>();
    }

    public void ApplyUpgrade(string upgradeType, int level)
    {
        switch (upgradeType)
        {
            case "ENGINE":
                stats.power += level * 10f;
                break;
            case "TURBO":
                stats.power += level * 15f;
                break;
            case "TIRES":
                stats.driftability += level * 0.1f;
                break;
            case "BRAKES":
                stats.braking += level * 20f;
                break;
            case "SUSPENSION":
                stats.handling += level * 0.05f;
                break;
        }
    }

    public void SetCarColor(Color newColor)
    {
        carColor = newColor;
        GetComponent<Renderer>().material.color = newColor;
    }

    public void ResetStats()
    {
        stats = new CarStats();
    }
}
