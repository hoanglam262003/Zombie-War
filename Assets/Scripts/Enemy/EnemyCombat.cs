using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private EnemyController controller;
    private PlayerController player;

    private float nextAttackTime;

    public bool IsAttacking { get; private set; }

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    private void OnEnable()
    {
        nextAttackTime = 0f;

        IsAttacking = false;
    }

    private void Update()
    {
        if (player == null)
            return;

        if (controller.Health.IsDead)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position);

        if (distance > attackRange)
        {
            IsAttacking = false;
            return;
        }

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        Attack();
    }

    private void Attack()
    {
        IsAttacking = true;

        controller.Animation.PlayAttack();
    }

    public void DealDamage()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.transform.position);

        if (distance > attackRange)
            return;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        player.Health.TakeDamage(
            damage,
            direction);
    }

    public void FinishAttack()
    {
        IsAttacking = false;
    }
}