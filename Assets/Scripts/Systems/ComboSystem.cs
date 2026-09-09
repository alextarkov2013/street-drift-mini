using UnityEngine;
using System;

public class ComboSystem : MonoBehaviour
{
    public int currentCombo { get; private set; } = 1;
    public Action<int> OnComboChanged;
    public Action OnComboReset;

    private DriftSystem driftSystem;
    private CarController carController;
    private float timeSinceLastDrift = 0f;
    private float comboTimeout = 3f;

    private void Awake()
    {
        driftSystem = GetComponent<DriftSystem>();
        carController = GetComponent<CarController>();
    }

    private void Start()
    {
        if (driftSystem != null)
        {
            driftSystem.OnDriftComplete += OnDriftCompleted;
        }
    }

    private void Update()
    {
        if (currentCombo > 1)
        {
            timeSinceLastDrift += Time.deltaTime;
            if (timeSinceLastDrift > comboTimeout)
            {
                ResetCombo();
            }
        }
    }

    private void OnDriftCompleted(DriftSystem.DriftData drift)
    {
        if (drift.driftAngle < 15f || drift.driftDuration < 0.5f)
        {
            ResetCombo();
            return;
        }

        IncreaseCombo();
    }

    private void IncreaseCombo()
    {
        currentCombo = Mathf.Min(currentCombo + 1, 5);
        timeSinceLastDrift = 0f;
        OnComboChanged?.Invoke(currentCombo);
    }

    public void ResetCombo()
    {
        if (currentCombo > 1)
        {
            currentCombo = 1;
            timeSinceLastDrift = 0f;
            OnComboReset?.Invoke();
            OnComboChanged?.Invoke(currentCombo);
        }
    }

    public string GetComboText()
    {
        return currentCombo switch
        {
            1 => "DRIFT",
            2 => "COMBO x2",
            3 => "COMBO x3",
            4 => "COMBO x4",
            5 => "PERFECT DRIFT",
            _ => "DRIFT"
        };
    }
}
