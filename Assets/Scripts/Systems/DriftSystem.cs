using UnityEngine;
using System;

public class DriftSystem : MonoBehaviour
{
    [System.Serializable]
    public class DriftData
    {
        public float driftDuration = 0f;
        public float driftAngle = 0f;
        public float driftSpeed = 0f;
        public float driftDistance = 0f;
        public int comboMultiplier = 1;
        public float totalScore = 0f;
    }

    public DriftData currentDrift = new DriftData();
    public Action<DriftData> OnDriftComplete;

    private bool isActive = false;
    private float driftStartTime = 0f;
    private Vector3 driftStartPosition = Vector3.zero;
    private CarController carController;

    [SerializeField] private float minDriftAngle = 15f;
    [SerializeField] private float minDriftDuration = 0.5f;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }

    public void OnDriftStart()
    {
        if (isActive) return;

        isActive = true;
        driftStartTime = Time.time;
        driftStartPosition = transform.position;
        currentDrift = new DriftData();
    }

    public void OnDriftEnd()
    {
        if (!isActive) return;

        currentDrift.driftDuration = Time.time - driftStartTime;

        if (currentDrift.driftDuration >= minDriftDuration && 
            currentDrift.driftAngle >= minDriftAngle)
        {
            CalculateDriftScore();
            OnDriftComplete?.Invoke(currentDrift);
        }

        isActive = false;
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        currentDrift.driftSpeed = carController.GetCurrentSpeed();
        currentDrift.driftDistance = Vector3.Distance(transform.position, driftStartPosition);
        CalculateDriftAngle();
    }

    private void CalculateDriftAngle()
    {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if (velocity.magnitude < 0.1f) return;

        float angle = Vector3.Angle(transform.forward, velocity.normalized);
        currentDrift.driftAngle = Mathf.Max(currentDrift.driftAngle, angle);
    }

    private void CalculateDriftScore()
    {
        float score = 0f;

        // Base drift score
        score += currentDrift.driftDuration * 100f;

        // Angle bonus
        score += Mathf.Clamp(currentDrift.driftAngle / 90f, 0f, 1f) * 200f;

        // Speed bonus
        score += (currentDrift.driftSpeed / 50f) * 150f;

        // Distance bonus
        score += (currentDrift.driftDistance / 100f) * 100f;

        currentDrift.totalScore = score * currentDrift.comboMultiplier;
    }

    public void IncreaseCombo()
    {
        currentDrift.comboMultiplier = Mathf.Min(currentDrift.comboMultiplier + 1, 5);
    }

    public void ResetCombo()
    {
        currentDrift.comboMultiplier = 1;
    }
}
