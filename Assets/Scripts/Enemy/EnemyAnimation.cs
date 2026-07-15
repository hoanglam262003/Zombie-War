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

    private void OnEnable()
    {
        animator.SetBool(DeadHash, false);

        animator.ResetTrigger(HitHash);
        animator.ResetTrigger(AttackHash);

        animator.Rebind();
        animator.Update(0f);
    }

    private void Update()
    {
        float speed = 0f;

        if (controller.NavMeshAgent.enabled)
        {
            speed = controller.NavMeshAgent.velocity.magnitude;
        }

        animator.SetFloat(SpeedHash, speed);
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