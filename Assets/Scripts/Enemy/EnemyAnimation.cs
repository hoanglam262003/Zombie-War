using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyController controller;
    private Animator animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");

    private static readonly int AttackHash = Animator.StringToHash("Attack");

    private static readonly int HitHash = Animator.StringToHash("Hit");

    private static readonly int DeadHash = Animator.StringToHash("Dead");

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        animator = controller.Animator;
    }

    private void Update()
    {
        animator.SetFloat(
            SpeedHash,
            controller.NavMeshAgent.velocity.magnitude);
    }
    public void PlayAttack()
    {
        animator.SetTrigger(AttackHash);
    }

    public void PlayHit()
    {
        animator.SetTrigger(HitHash);
    }

    public void PlayDeath()
    {
        animator.SetBool(DeadHash, true);
    }

    public void OnAttackHit()
    {
        controller.Combat.DealDamage();
    }

    public void OnAttackFinished()
    {
        controller.Combat.FinishAttack();
    }
}