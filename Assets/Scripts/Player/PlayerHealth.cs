using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    private PlayerController controller;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDead;

    public int MaxHealth => maxHealth;

    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if (IsDead)
            return;

        damage = Mathf.Max(0, damage);

        CurrentHealth -= damage;
        SpawnBloodEffect(hitDirection);

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        controller.Animation.PlayHit();
        controller.Movement.LockMovement(0.5f);
        controller.Combat.LockShoot(0.5f);

        if (CurrentHealth > 0)
            return;

        Die();
    }

    private void SpawnBloodEffect(Vector3 hitDirection)
    {
        Vector3 position =
            transform.position +
            Vector3.up;

        hitDirection.y = 0f;

        if (hitDirection.sqrMagnitude < 0.001f)
            hitDirection = transform.forward;

        Quaternion rotation =
            Quaternion.LookRotation(-hitDirection);

        ObjectPoolManager.Instance.Get(
            PoolType.BloodEffect,
            position,
            rotation);
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        amount = Mathf.Max(0, amount);

        CurrentHealth += amount;

        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void FullHeal()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Die()
    {
        controller.Weapon.UnequipWeapon();
        controller.Animation.SetDead(true);

        OnDead?.Invoke();
    }
}