using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;

    [Header("Systems")]
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemyAnimation animationController;
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyCombat combat;

    public NavMeshAgent NavMeshAgent => navMeshAgent;
    public Animator Animator => animator;

    public EnemyMovement Movement => movement;
    public EnemyAnimation Animation => animationController;
    public EnemyHealth Health => health;
    public EnemyCombat Combat => combat;

    private void Awake()
    {
        navMeshAgent ??= GetComponent<NavMeshAgent>();
        animator ??= GetComponent<Animator>();

        movement ??= GetComponent<EnemyMovement>();
        animationController ??= GetComponent<EnemyAnimation>();
        health ??= GetComponent<EnemyHealth>();
        combat ??= GetComponent<EnemyCombat>();
    }
}