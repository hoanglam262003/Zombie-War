using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController controller;

    public bool IsAiming =>
    controller.GameInput != null &&
    controller.GameInput.IsAimPressed;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
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
        if (!value)
            return;

        WeaponBehaviour behaviour =
            controller.Weapon.CurrentBehaviour;

        if (behaviour == null)
            return;

        behaviour.Attack();
    }
}