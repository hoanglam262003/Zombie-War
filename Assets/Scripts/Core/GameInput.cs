using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event Action<WeaponType> OnWeaponChanged;
    public event Action<bool> OnAimStateChanged;
    public event Action<bool> OnShootStateChanged;
    public event Action<bool> OnRunStateChanged;

    public Vector2 MoveInput { get; private set; }

    public Vector2 AimDirection { get; private set; }

    public bool IsAimPressed { get; private set; }

    public bool IsShootPressed { get; private set; }

    public bool IsRunning { get; private set; }

    public WeaponType CurrentWeapon { get; private set; } = WeaponType.Unarmed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetMoveInput(Vector2 value)
    {
        MoveInput = value;
    }

    public void SetAimDirection(Vector2 value)
    {
        AimDirection = value;
    }

    public void SetAimState(bool value)
    {
        if (IsAimPressed == value)
            return;

        IsAimPressed = value;
        OnAimStateChanged?.Invoke(value);
    }

    public void SetShootState(bool value)
    {
        if (IsShootPressed == value)
            return;

        IsShootPressed = value;
        OnShootStateChanged?.Invoke(value);
    }

    public void SetRunState(bool value)
    {
        if (IsRunning == value)
            return;

        IsRunning = value;
        OnRunStateChanged?.Invoke(value);
    }

    public void SelectWeapon(WeaponType weapon)
    {
        if (CurrentWeapon == weapon)
            return;

        CurrentWeapon = weapon;
        OnWeaponChanged?.Invoke(CurrentWeapon);
    }
}