using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] private WeaponObject[] weapons;

    private readonly Dictionary<WeaponType, WeaponObject> weaponLookup = new();

    private PlayerController controller;

    public WeaponType CurrentWeapon { get; private set; } = WeaponType.Unarmed;

    public WeaponObject CurrentWeaponObject { get; private set; }

    public WeaponBehaviour CurrentBehaviour { get; private set; }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();

        weaponLookup.Clear();

        foreach (WeaponObject weapon in weapons)
        {
            if (weapon == null)
                continue;

            if (weaponLookup.ContainsKey(weapon.WeaponType))
            {
                Debug.LogWarning($"Duplicate weapon type : {weapon.WeaponType}");
                continue;
            }

            weaponLookup.Add(weapon.WeaponType, weapon);
        }
    }

    private void Start()
    {
        InitializeWeapons();

        EquipWeapon(controller.GameInput.CurrentWeapon);
    }

    private void OnEnable()
    {
        if (controller.GameInput != null)
            controller.GameInput.OnWeaponChanged += EquipWeapon;
    }

    private void OnDisable()
    {
        if (controller == null || controller.GameInput == null)
            return;

        controller.GameInput.OnWeaponChanged -= EquipWeapon;
    }

    private void InitializeWeapons()
    {
        foreach (WeaponObject weapon in weaponLookup.Values)
        {
            if (weapon.Behaviour != null)
            {
                weapon.Behaviour.Initialize(controller);
            }

            if (weapon.Holder != null)
            {
                weapon.Holder.SetActive(false);
            }
        }
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        CurrentWeapon = weaponType;

        foreach (WeaponObject weapon in weaponLookup.Values)
        {
            if (weapon.Holder != null)
            {
                weapon.Holder.SetActive(false);
            }
        }

        CurrentWeaponObject = null;
        CurrentBehaviour = null;

        if (!weaponLookup.TryGetValue(weaponType, out WeaponObject weaponObject))
            return;

        CurrentWeaponObject = weaponObject;
        CurrentBehaviour = weaponObject.Behaviour;

        if (CurrentBehaviour != null)
        {
            CurrentBehaviour.Initialize(controller);
        }

        if (weaponObject.Holder != null)
        {
            weaponObject.Holder.SetActive(true);
        }
    }

    public bool IsEquipped(WeaponType weaponType)
    {
        return CurrentWeapon == weaponType;
    }

    public void UnequipWeapon()
    {
        foreach (WeaponObject weapon in weaponLookup.Values)
        {
            if (weapon.Holder != null)
            {
                weapon.Holder.SetActive(false);
            }
        }

        CurrentWeapon = WeaponType.Unarmed;
        CurrentWeaponObject = null;
        CurrentBehaviour = null;
    }
}