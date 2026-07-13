using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBridge : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private GameInput gameInput;

    private Camera mainCamera;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        gameInput = GameInput.Instance;
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Move
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        // Aim Button
        inputActions.Player.Aim.performed += OnAim;
        inputActions.Player.Aim.canceled += OnAim;

        // Aim Direction
        inputActions.Player.AimDirection.performed += OnAimDirection;
        inputActions.Player.AimDirection.canceled += OnAimDirection;

        // Shoot
        inputActions.Player.Shoot.performed += OnShootStarted;
        inputActions.Player.Shoot.canceled += OnShootCanceled;

        // Run
        inputActions.Player.RunOnOff.performed += OnRunToggle;

        // Weapons
        inputActions.Player.Unarmed.performed += OnUnarmed;
        inputActions.Player.Rifle.performed += OnWeapon1;
        inputActions.Player.Pistol.performed += OnWeapon2;
        inputActions.Player.Grenade.performed += OnWeapon3;
    }

    private void OnDisable()
    {
        // Move
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        // Aim
        inputActions.Player.Aim.performed -= OnAim;
        inputActions.Player.Aim.canceled -= OnAim;

        // Aim Direction
        inputActions.Player.AimDirection.performed -= OnAimDirection;
        inputActions.Player.AimDirection.canceled -= OnAimDirection;

        // Shoot
        inputActions.Player.Shoot.performed -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;

        // Run
        inputActions.Player.RunOnOff.performed -= OnRunToggle;

        // Weapons
        inputActions.Player.Unarmed.performed -= OnUnarmed;
        inputActions.Player.Rifle.performed -= OnWeapon1;
        inputActions.Player.Pistol.performed -= OnWeapon2;
        inputActions.Player.Grenade.performed -= OnWeapon3;

        inputActions.Disable();
    }

    private void Update()
    {
#if UNITY_STANDALONE || UNITY_EDITOR

        if (gameInput != null && gameInput.IsAimPressed)
        {
            Vector2 aim = GetMouseAimDirection();
            gameInput.SetAimDirection(aim);
        }

#endif
    }

    //--------------------------------------------------
    // MOVE
    //--------------------------------------------------

    private void OnMove(InputAction.CallbackContext context)
    {
        gameInput.SetMoveInput(context.ReadValue<Vector2>());
    }

    //--------------------------------------------------
    // AIM BUTTON
    //--------------------------------------------------

    private void OnAim(InputAction.CallbackContext context)
    {
        gameInput.SetAimState(context.ReadValueAsButton());
    }

    //--------------------------------------------------
    // AIM DIRECTION
    //--------------------------------------------------

    private void OnAimDirection(InputAction.CallbackContext context)
    {
#if UNITY_ANDROID || UNITY_IOS

        Vector2 direction = context.ReadValue<Vector2>();

        gameInput.SetAimDirection(direction);

        gameInput.SetAimState(direction.sqrMagnitude > 0.01f);

#endif
    }

    //--------------------------------------------------
    // SHOOT
    //--------------------------------------------------

    private void OnShootStarted(InputAction.CallbackContext context)
    {
        gameInput.SetShootState(true);
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        gameInput.SetShootState(false);
    }

    //--------------------------------------------------
    // RUN
    //--------------------------------------------------

    private void OnRunToggle(InputAction.CallbackContext context)
    {
        gameInput.SetRunState(!gameInput.IsRunning);
    }

    //--------------------------------------------------
    // WEAPONS
    //--------------------------------------------------

    private void OnUnarmed(InputAction.CallbackContext context)
    {
        gameInput.SelectWeapon(WeaponType.Unarmed);
    }

    private void OnWeapon1(InputAction.CallbackContext context)
    {
        gameInput.SelectWeapon(WeaponType.Rifle);
    }

    private void OnWeapon2(InputAction.CallbackContext context)
    {
        gameInput.SelectWeapon(WeaponType.Pistol);
    }

    private void OnWeapon3(InputAction.CallbackContext context)
    {
        gameInput.SelectWeapon(WeaponType.Grenade);
    }

    //--------------------------------------------------
    // MOUSE AIM
    //--------------------------------------------------

    private Vector2 GetMouseAimDirection()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mouseScreen);

        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hit = ray.GetPoint(distance);

            Vector3 direction = hit - transform.position;

            direction.y = 0;

            return new Vector2(direction.x, direction.z).normalized;
        }

        return Vector2.zero;
    }
}