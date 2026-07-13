using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerController controller;
    private Animator animator;

    #region Animator Hashes

    private static readonly int MotionSpeedHash = Animator.StringToHash("MotionSpeed");
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int XHash = Animator.StringToHash("X");
    private static readonly int YHash = Animator.StringToHash("Y");

    private static readonly int WeaponHash = Animator.StringToHash("WeaponType");

    private static readonly int ShootHash = Animator.StringToHash("Shoot");
    private static readonly int ThrowHash = Animator.StringToHash("Throw");

    private static readonly int HitHash = Animator.StringToHash("Hit");
    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private static readonly int AimHash = Animator.StringToHash("Aiming");

    #endregion

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        animator = controller.Animator;
        for (int i = 0; i < animator.layerCount; i++)
        {
            Debug.Log(
              animator.GetLayerName(i)
              + " weight = "
              + animator.GetLayerWeight(i));
        }
    }

    private void Update()
    {
        UpdateMovementAnimation();
        UpdateWeaponAnimation();
    }

    private void UpdateMovementAnimation()
    {
        PlayerMovement movement = controller.Movement;

        animator.SetFloat(
            SpeedHash,
            movement.NormalizedSpeed,
            0.1f,
            Time.deltaTime);

        animator.SetFloat(
            MotionSpeedHash,
            movement.IsRunning ? 1f : 0.5f);

        Vector3 localMove =
            transform.InverseTransformDirection(movement.MoveDirection);

        animator.SetFloat(
            XHash,
            localMove.x,
            0.1f,
            Time.deltaTime);

        animator.SetFloat(
            YHash,
            localMove.z,
            0.1f,
            Time.deltaTime);
    }

    private void UpdateWeaponAnimation()
    {
        animator.SetInteger(
            WeaponHash,
            (int)controller.GameInput.CurrentWeapon);
    }

    public void SetAim(bool aiming)
    {
        animator.SetBool(AimHash, aiming);
    }

    public void PlayShoot()
    {
        animator.SetTrigger(ShootHash);
    }

    public void PlayThrow()
    {
        animator.SetTrigger(ThrowHash);
    }

    public void PlayHit()
    {
        animator.SetTrigger(HitHash);
    }

    public void SetDead(bool dead)
    {
        animator.SetBool(DeadHash, dead);
    }
}