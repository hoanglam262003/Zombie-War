using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 1000f;
    [SerializeField]
    [Range(0f, 1f)]
    private float strafeSpeedMultiplier = 0.8f;

    private PlayerController controller;

    public Vector3 MoveDirection { get; private set; }

    public Vector3 FacingDirection { get; private set; } = Vector3.forward;

    public float MoveSpeed { get; private set; }

    public float NormalizedSpeed { get; private set; }

    public bool IsMoving => MoveDirection.sqrMagnitude > 0.001f;
    public bool IsRunning { get; private set; }

    private float movementLockTimer;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Tick(Vector2 moveInput, bool isRunning, bool isAim, Vector2 aimInput)
    {
        if (movementLockTimer > 0f)
        {
            movementLockTimer -= Time.deltaTime;
        }
        CalculateMoveDirection(moveInput);

        HandleMovement(isRunning);

        HandleRotation(isAim, aimInput);

        UpdateState(isRunning);
    }

    private void CalculateMoveDirection(Vector2 input)
    {
        MoveDirection = new Vector3(input.x, 0f, input.y);

        if (MoveDirection.sqrMagnitude > 1f)
            MoveDirection.Normalize();
    }

    private void HandleMovement(bool isRunning)
    {
        if (movementLockTimer > 0f)
            return;
        MoveSpeed = isRunning ? runSpeed : walkSpeed;
        if (controller.Combat.IsAiming || controller.Combat.isShooting)
        {
            MoveSpeed *= strafeSpeedMultiplier;
        }
        controller.CharacterController.Move(
            MoveDirection * MoveSpeed * Time.deltaTime);
    }

    private void HandleRotation(bool isAim, Vector2 aimInput)
    {
        Vector3 targetForward = FacingDirection;

        if (isAim)
        {
            if (Application.isMobilePlatform)
            {
                Vector3 aimDirection = new Vector3(
                    aimInput.x,
                    0f,
                    aimInput.y);

                if (aimDirection.sqrMagnitude > 0.01f)
                {
                    targetForward = aimDirection.normalized;
                }
            }
            else
            {
                Ray ray =
                    Camera.main.ScreenPointToRay(
                        Mouse.current.position.ReadValue());

                Plane ground =
                    new Plane(Vector3.up, Vector3.zero);

                if (ground.Raycast(ray, out float enter))
                {
                    Vector3 hitPoint =
                        ray.GetPoint(enter);

                    targetForward =
                        hitPoint - transform.position;

                    targetForward.y = 0f;

                    if (targetForward.sqrMagnitude > 0.01f)
                        targetForward.Normalize();
                }
            }
        }
        else if (IsMoving)
        {
            targetForward = MoveDirection;
        }

        if (targetForward.sqrMagnitude < 0.001f)
            return;

        FacingDirection = targetForward;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                FacingDirection,
                Vector3.up);

        transform.rotation =
            Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
    }

    private void UpdateState(bool isRunning)
    {
        IsRunning = isRunning;
        if (!IsMoving)
        {
            MoveSpeed = 0f;
            NormalizedSpeed = 0f;
            return;
        }

        float speed = isRunning ? runSpeed : walkSpeed;

        if (controller.Combat.IsAiming || controller.Combat.isShooting)
        {
            speed *= strafeSpeedMultiplier;
        }

        MoveSpeed = speed;
        NormalizedSpeed = speed;
    }

    public void LockMovement(float duration)
    {
        movementLockTimer = duration;
    }
}