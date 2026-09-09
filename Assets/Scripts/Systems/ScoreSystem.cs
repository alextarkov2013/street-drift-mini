using UnityEngine;
using System;

public class ScoreSystem : MonoBehaviour
{
    [System.Serializable]
    public class RaceResult
    {
        public float score = 0f;
        public int bestCombo = 1;
        public float totalDriftDistance = 0f;
        public float moneyEarned = 0f;
        public float maxSpeed = 0f;
    }

    public RaceResult currentRace = new RaceResult();
    public Action<RaceResult> OnRaceEnd;

    private DriftSystem driftSystem;
    private CarController carController;
    private float sessionStartTime = 0f;

    [SerializeField] private float driftReward = 100f;
    [SerializeField] private float comboReward = 250f;
    [SerializeField] private float speedReward = 100f;
    [SerializeField] private float perfectDriftReward = 500f;

    private void Awake()
    {
        driftSystem = GetComponent<DriftSystem>();
        carController = GetComponent<CarController>();
    }

    private void Start()
    {
        sessionStartTime = Time.time;
        if (driftSystem != null)
        {
            driftSystem.OnDriftComplete += OnDriftCompleted;
        }
    }

    private void OnDriftCompleted(DriftSystem.DriftData drift)
    {
        currentRace.score += drift.totalScore;
        currentRace.totalDriftDistance += drift.driftDistance;

        // Update best combo
        if (drift.comboMultiplier > currentRace.bestCombo)
        {
            currentRace.bestCombo = drift.comboMultiplier;
        }

        // Calculate money
        float money = driftReward;
        money += (drift.comboMultiplier - 1) * comboReward;
        money += (drift.driftSpeed / 50f) * speedReward;

        if (drift.comboMultiplier >= 5)
        {
            money += perfectDriftReward;
        }

        currentRace.moneyEarned += money;
    }

    private void Update()
    {
        if (carController != null)
        {
            currentRace.maxSpeed = Mathf.Max(currentRace.maxSpeed, carController.GetCurrentSpeed());
        }
    }

    public void EndRace()
    {
        OnRaceEnd?.Invoke(currentRace);
    }
}
