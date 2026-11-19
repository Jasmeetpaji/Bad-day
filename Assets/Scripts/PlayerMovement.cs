using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Engine")]
    public float acceleration = 20f;
    public float maxSpeed = 25f;
    [Header("Steering")]
    public float steeringSpeed = 100f;
    public float steeringAtLowSpeed = 0.3f;
    [Header("Brakes")]
    public float brakeForce = 35f;
    public float normalDrag = 0.2f;
    public float handbrakeDrag = 3f;
    private Rigidbody rb;
    private float accelerationInput;
    private float steeringInput;
    private bool braking;
    private bool handbraking;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = normalDrag;
    }
    void Update()
    {
        if (Keyboard.current == null)
            return;
        accelerationInput = Keyboard.current.wKey.isPressed ? 1f : 0f;
        braking = Keyboard.current.sKey.isPressed;
        steeringInput = 0f;
        if (Keyboard.current.aKey.isPressed)
            steeringInput = -1f;
        if (Keyboard.current.dKey.isPressed)
            steeringInput = 1f;
        handbraking = Keyboard.current.spaceKey.isPressed;
    }
    void FixedUpdate()
    {
        Drive();
        Steer();
        Brake();
    }
    void Drive()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (accelerationInput > 0f && forwardSpeed < maxSpeed)
        {
            rb.AddForce(
                transform.forward * acceleration * accelerationInput,
                ForceMode.Acceleration
            );
        }
    }
    void Steer()
    {
        float speed = rb.linearVelocity.magnitude;
        if (speed < 0.5f)
            return;

        float speedFactor = Mathf.Clamp01(speed / 6f);
        float turn = steeringInput * steeringSpeed *
                     Mathf.Max(steeringAtLowSpeed, speedFactor) *
                     Time.fixedDeltaTime;
        rb.MoveRotation(
            rb.rotation * Quaternion.Euler(0f, turn, 0f)
        );
    }
    void Brake()
    {
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (braking && forwardSpeed > 0.1f)
        {
            rb.AddForce(
                -transform.forward * brakeForce,
                ForceMode.Acceleration
            );
        }
        rb.linearDamping = handbraking ? handbrakeDrag : normalDrag;
    }
}