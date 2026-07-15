using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController controller;

    public bool IsAiming =>
    controller.GameInput != null &&
    controller.GameInput.IsAimPressed;

    private bool isShooting;
    private float shootLockTimer;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (shootLockTimer > 0f)
        {
            shootLockTimer -= Time.deltaTime;
            return;
        }
        if (!isShooting)
            return;

        WeaponBehaviour behaviour =
            controller.Weapon.CurrentBehaviour;

        if (behaviour == null)
            return;

        behaviour.Attack();
    }

    private void OnEnable()
    {
        controller.GameInput.OnAimStateChanged += HandleAim;

        controller.GameInput.OnShootStateChanged += HandleShoot;
    }

    private void OnDisable()
    {
        if (controller == null || controller.GameInput == null)
            return;

        controller.GameInput.OnAimStateChanged -= HandleAim;

        controller.GameInput.OnShootStateChanged -= HandleShoot;
    }

    private void HandleAim(bool value)
    {
        WeaponBehaviour behaviour =
            controller.Weapon.CurrentBehaviour;

        if (behaviour == null)
            return;

        if (value)
            behaviour.BeginAim();
        else
            behaviour.EndAim();
    }

    private void HandleShoot(bool value)
    {
        isShooting = value;

        WeaponBehaviour behaviour =
            controller.Weapon.CurrentBehaviour;

        if (behaviour == null)
            return;

        if (value)
        {
            behaviour.BeginFire();

            if (controller.Weapon.CurrentWeapon == WeaponType.Pistol)
            {
                behaviour.Attack();
            }
        }
        else
        {
            behaviour.EndFire();
        }
    }
    public void LockShoot(float duration)
    {
        shootLockTimer = duration;
        isShooting = false;
        controller.Weapon.CurrentBehaviour?.EndFire();
    }
}