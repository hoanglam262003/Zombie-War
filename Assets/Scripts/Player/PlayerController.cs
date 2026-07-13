using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;

    [Header("Systems")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerAnimation animationController;
    [SerializeField] private PlayerCombat combat;
    [SerializeField] private PlayerWeapon weapon;
    [SerializeField] private PlayerHealth health;

    [Header("Input")]
    [SerializeField] private GameInput gameInput;

    public CharacterController CharacterController => characterController;
    public Animator Animator => animator;

    public PlayerMovement Movement => movement;
    public PlayerAnimation Animation => animationController;
    public PlayerCombat Combat => combat;
    public PlayerWeapon Weapon => weapon;
    public PlayerHealth Health => health;

    public GameInput GameInput => gameInput;

    private void Awake()
    {
        characterController ??= GetComponent<CharacterController>();
        animator ??= GetComponent<Animator>();

        movement ??= GetComponent<PlayerMovement>();
        animationController ??= GetComponent<PlayerAnimation>();
        combat ??= GetComponent<PlayerCombat>();
        weapon ??= GetComponent<PlayerWeapon>();
        health ??= GetComponent<PlayerHealth>();

        gameInput ??= GameInput.Instance;
    }

    private void Update()
    {
        if (gameInput == null)
            return;

        movement?.Tick(
            gameInput.MoveInput,
            gameInput.IsRunning,
            gameInput.IsAimPressed,
            gameInput.AimDirection);
    }
}