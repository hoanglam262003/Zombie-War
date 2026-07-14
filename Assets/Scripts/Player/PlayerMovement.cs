using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 1000f;

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

        controller.CharacterController.Move(
            MoveDirection * MoveSpeed * Time.deltaTime);
    }

    private void HandleRotation(bool isAim, Vector2 aimInput)
    {
        Vector3 targetForward = FacingDirection;

        if (isAim)
        {
            Vector3 aimDirection = new Vector3(
                aimInput.x,
                0f,
                aimInput.y);

            if (aimDirection.sqrMagnitude > 0.001f)
            {
                targetForward = aimDirection.normalized;
            }
            else if (IsMoving)
            {
                targetForward = MoveDirection;
            }
        }
        else
        {
            if (IsMoving)
            {
                targetForward = MoveDirection;
            }
        }

        FacingDirection = targetForward;

        Quaternion targetRotation =
            Quaternion.LookRotation(FacingDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(
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

        if (isRunning)
        {
            MoveSpeed = runSpeed;
            NormalizedSpeed = 6f;
        }
        else
        {
            MoveSpeed = walkSpeed;
            NormalizedSpeed = 2f;
        }
    }

    public void LockMovement(float duration)
    {
        movementLockTimer = duration;
    }
}