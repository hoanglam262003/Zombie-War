using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private float returnToPoolDelay = 3f;
    private EnemyController controller;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDead;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();

        CurrentHealth = maxHealth;
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        CurrentHealth = maxHealth;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        damage = Mathf.Max(0, damage);

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        controller.Animation.PlayHit();

        if (CurrentHealth <= 0)
            Die();
    }

    private void Die()
    {
        controller.Animation.PlayDeath();

        controller.NavMeshAgent.enabled = false;
        controller.Combat.enabled = false;
        controller.Movement.enabled = false;

        OnDead?.Invoke();
        StartCoroutine(ReturnToPoolAfterDelay());
    }
    private IEnumerator ReturnToPoolAfterDelay()
    {
        yield return new WaitForSeconds(returnToPoolDelay);

        ObjectPoolManager.Instance.Return(gameObject);
    }
}