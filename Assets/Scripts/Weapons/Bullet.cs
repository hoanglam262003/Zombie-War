using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PoolObject))]
public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float lifeTime = 3f;

    [Header("Collision")]
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody rb;

    private int damage;

    private float speed;

    private float timer;

    private bool initialized;

    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        timer = 0f;

        initialized = false;

        hasHit = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (!initialized)
            return;

        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            ReturnToPool();
            return;
        }

        Vector3 velocity =
            rb.linearVelocity;

        velocity.y = 0f;

        if (velocity.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    velocity.normalized,
                    Vector3.up);
        }
    }

    public void Initialize(int damage, float speed, Vector3 direction)
    {
        this.damage = damage;
        this.speed = speed;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            direction = Vector3.forward;

        direction.Normalize();

        initialized = true;

        transform.rotation =
            Quaternion.LookRotation(
                direction,
                Vector3.up);

        rb.linearVelocity =
            direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!initialized)
            return;

        if (hasHit)
            return;

        hasHit = true;

        EnemyHealth enemy =
            other.GetComponentInParent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            //SpawnBloodEffect();

            ReturnToPool();

            return;
        }

        if (((1 << other.gameObject.layer) & obstacleLayer) != 0)
        {
            ReturnToPool();
        }
    }

    private void SpawnBloodEffect()
    {
        ObjectPoolManager.Instance.Get(
            PoolType.BloodEffect,
            transform.position,
            Quaternion.identity);
    }

    private void ReturnToPool()
    {
        initialized = false;

        hasHit = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        ObjectPoolManager.Instance.Return(gameObject);
    }
}