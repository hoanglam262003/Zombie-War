using UnityEngine;
using UnityEngine.UI;

public class MobileInputUI : MonoBehaviour
{
    [Header("Joysticks")]
    [SerializeField]
    private FixedJoystick movementJoystick;

    [SerializeField]
    private FloatingJoystick aimJoystick;
    [SerializeField]
    private GameObject aimJoystickRoot;

    [Header("Buttons")]
    [SerializeField]
    private UIButtonHold shootButton;

    [SerializeField]
    private Button runButton;

    [SerializeField]
    private Button rifleButton;

    [SerializeField]
    private Button pistolButton;

    [SerializeField]
    private Button grenadeButton;

    [SerializeField]
    private Button unarmedButton;

    [Header("Button Images")]
    [SerializeField] private Image runButtonImage;

    [SerializeField] private Image rifleButtonImage;

    [SerializeField] private Image pistolButtonImage;

    [SerializeField] private Image grenadeButtonImage;

    [SerializeField] private Image unarmedButtonImage;

    [Header("Button Colors")]
    [SerializeField] private Color normalColor = Color.white;

    [SerializeField] private Color selectedColor = Color.green;

    private GameInput gameInput;

    private bool runEnabled;
    private void Start()
    {
        gameInput = GameInput.Instance;
        if (gameInput == null)
            return;
        gameInput.OnWeaponChanged += UpdateWeaponButtons;
        gameInput.OnRunStateChanged += UpdateRunButton;
        UpdateWeaponButtons(gameInput.CurrentWeapon);
        UpdateRunButton(gameInput.IsRunning);
    }

    private void OnEnable()
    {
        shootButton.OnPressed += ShootPressed;
        shootButton.OnReleased += ShootReleased;

        runButton.onClick.AddListener(OnRunClicked);

        rifleButton.onClick.AddListener(SelectRifle);
        pistolButton.onClick.AddListener(SelectPistol);
        grenadeButton.onClick.AddListener(SelectGrenade);
        unarmedButton.onClick.AddListener(SelectUnarmed);
    }

    private void OnDisable()
    {
        shootButton.OnPressed -= ShootPressed;
        shootButton.OnReleased -= ShootReleased;

        runButton.onClick.RemoveListener(OnRunClicked);

        rifleButton.onClick.RemoveListener(SelectRifle);
        pistolButton.onClick.RemoveListener(SelectPistol);
        grenadeButton.onClick.RemoveListener(SelectGrenade);
        unarmedButton.onClick.RemoveListener(SelectUnarmed);
        if (gameInput != null)
        {
            gameInput.OnWeaponChanged -= UpdateWeaponButtons;
            gameInput.OnRunStateChanged -= UpdateRunButton;
        }
    }

    private void Update()
    {
        if (gameInput == null)
            return;
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR

        UpdateMovement();
        UpdateAim();
#endif
    }

    private void UpdateMovement()
    {
        if (movementJoystick == null)
            return;
        gameInput.SetMoveInput(movementJoystick.Direction);
    }

    private void UpdateAim()
    {
        if (aimJoystick == null)
            return;
        Vector2 dir = aimJoystick.Direction;

        gameInput.SetAimDirection(dir);

        gameInput.SetAimState(dir.sqrMagnitude > 0.01f);
    }

    private void ShootPressed()
    {
        gameInput.SetShootState(true);
    }

    private void ShootReleased()
    {
        gameInput.SetShootState(false);
    }

    private void OnRunClicked()
    {
        runEnabled = !runEnabled;

        gameInput.SetRunState(runEnabled);
    }

    private void SelectRifle()
    {
        gameInput.SelectWeapon(WeaponType.Rifle);
    }

    private void SelectPistol()
    {
        gameInput.SelectWeapon(WeaponType.Pistol);
    }

    private void SelectGrenade()
    {
        gameInput.SelectWeapon(WeaponType.Grenade);
    }

    private void SelectUnarmed()
    {
        gameInput.SelectWeapon(WeaponType.Unarmed);
    }

    private void UpdateWeaponButtons(WeaponType weapon)
    {
        rifleButtonImage.color =
            weapon == WeaponType.Rifle
            ? selectedColor
            : normalColor;

        pistolButtonImage.color =
            weapon == WeaponType.Pistol
            ? selectedColor
            : normalColor;

        grenadeButtonImage.color =
            weapon == WeaponType.Grenade
            ? selectedColor
            : normalColor;

        unarmedButtonImage.color =
            weapon == WeaponType.Unarmed
            ? selectedColor
            : normalColor;
    }

    private void UpdateRunButton(bool isRunning)
    {
        runButtonImage.color =
            isRunning
            ? selectedColor
            : normalColor;
    }
}