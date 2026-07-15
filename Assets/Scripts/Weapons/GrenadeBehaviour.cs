using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PoolObject))]
public class GrenadeBehaviour : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField]
    private float explosionDelay = 3f;

    [SerializeField]
    private float explosionRadius = 4f;

    [SerializeField]
    private int damage = 100;

    [SerializeField]
    private LayerMask zombieLayer;

    [SerializeField]
    private GameObject audioSourcePrefab;

    [SerializeField]
    private AudioClip impactSound;

    [SerializeField]
    private AudioClip[] explosionSounds;

    private Rigidbody rb;

    private AudioSource impactAudio;

    private float timer;

    private bool initialized;

    private bool exploded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        impactAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        timer = 0f;

        initialized = false;

        exploded = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Throw(Vector3 velocity)
    {
        initialized = true;

        rb.linearVelocity = velocity;
    }

    private void Update()
    {
        if (!initialized)
            return;

        if (exploded)
            return;

        timer += Time.deltaTime;

        if (timer >= explosionDelay)
        {
            Explode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded)
            return;

        if (collision.collider.CompareTag("Player"))
            return;

        PlayImpactSound();
    }

    private void Explode()
    {
        exploded = true;

        SpawnExplosionEffect();

        PlayExplosionSound();

        DealDamage();

        ObjectPoolManager.Instance.Return(gameObject);
    }

    private void DealDamage()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                explosionRadius,
                zombieLayer);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy =
                hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                Vector3 direction = (enemy.transform.position - transform.position).normalized;

                enemy.TakeDamage(damage, direction);
            }
        }
    }

    private void SpawnExplosionEffect()
    {
        ObjectPoolManager.Instance.Get(
            PoolType.ExplosionEffect,
            transform.position,
            Quaternion.identity);
    }

    private void PlayImpactSound()
    {
        if (impactAudio == null)
            return;

        if (impactSound == null)
            return;

        impactAudio.spatialBlend = 1f;
        impactAudio.clip = impactSound;
        impactAudio.Play();
    }

    private void PlayExplosionSound()
    {
        if (audioSourcePrefab == null)
            return;

        GameObject obj =
            Instantiate(
                audioSourcePrefab,
                transform.position,
                Quaternion.identity);

        AudioSource source =
            obj.GetComponent<AudioSource>();

        if (source == null)
        {
            Destroy(obj);
            return;
        }

        source.spatialBlend = 1f;

        if (explosionSounds != null &&
            explosionSounds.Length > 0)
        {
            int index =
                Random.Range(
                    0,
                    explosionSounds.Length);

            source.clip = explosionSounds[index];
        }

        if (source.clip != null)
        {
            source.Play();

            Destroy(
                obj,
                source.clip.length);
        }
        else
        {
            Destroy(obj);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius);
    }
#endif
}