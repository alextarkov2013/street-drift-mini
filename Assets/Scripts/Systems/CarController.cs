using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerCar playerCar;
    private DriftSystem driftSystem;

    private Vector3 moveDirection = Vector3.zero;
    private float currentSpeed = 0f;
    private float steerInput = 0f;
    private float throttleInput = 0f;
    private bool isDrifting = false;

    [SerializeField] private float wheelDragDrift = 0.05f;
    [SerializeField] private float wheelDragNormal = 2f;
    [SerializeField] private float steerSensitivity = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCar = GetComponent<PlayerCar>();
        driftSystem = GetComponent<DriftSystem>();
    }

    public void SetInput(float steering, float throttle, bool drift)
    {
        steerInput = Mathf.Clamp(steering, -1f, 1f);
        throttleInput = Mathf.Clamp(throttle, -1f, 1f);

        if (drift && !isDrifting)
        {
            StartDrift();
        }
        else if (!drift && isDrifting)
        {
            EndDrift();
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
        UpdateDrag();
        LimitSpeed();
    }

    private void ApplyMovement()
    {
        Vector3 forceDirection = transform.forward;
        float acceleration = throttleInput * playerCar.stats.power;
        rb.AddForce(forceDirection * acceleration, ForceMode.Acceleration);
    }

    private void ApplyRotation()
    {
        float steerAmount = steerInput * steerSensitivity * playerCar.stats.handling;
        
        if (isDrifting)
        {
            steerAmount *= 1.5f;
        }

        transform.Rotate(Vector3.up * steerAmount * Time.fixedDeltaTime);
    }

    private void UpdateDrag()
    {
        rb.drag = isDrifting ? wheelDragDrift : wheelDragNormal;
    }

    private void LimitSpeed()
    {
        currentSpeed = rb.velocity.magnitude;
        if (currentSpeed > playerCar.stats.maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * playerCar.stats.maxSpeed;
        }
    }

    private void StartDrift()
    {
        isDrifting = true;
        driftSystem?.OnDriftStart();
    }

    private void EndDrift()
    {
        isDrifting = false;
        driftSystem?.OnDriftEnd();
    }

    public float GetCurrentSpeed() => currentSpeed;
    public bool IsDrifting() => isDrifting;
}
