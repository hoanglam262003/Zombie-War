using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float stoppingDistance = 1.5f;

    private EnemyController controller;
    private PlayerController player;

    public bool HasTarget => player != null;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        controller.NavMeshAgent.stoppingDistance = stoppingDistance;
    }

    private void OnEnable()
    {
        controller.NavMeshAgent.ResetPath();

        controller.NavMeshAgent.isStopped = false;
    }

    private void Update()
    {
        if (player == null)
            return;
        if (controller.Health.IsDead)
            return;

        if (controller.Combat.IsAttacking)
        {
            controller.NavMeshAgent.isStopped = true;
            return;
        }
        controller.NavMeshAgent.isStopped = false;
        controller.NavMeshAgent.SetDestination(
            player.transform.position);

        RotateTowardsPlayer();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction =
            player.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion rotation =
            Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotation,
            10f * Time.deltaTime);
    }
}