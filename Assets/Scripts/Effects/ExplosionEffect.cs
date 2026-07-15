using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystemEffect;

    private void Awake()
    {
        if (particleSystemEffect == null)
            particleSystemEffect = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        particleSystemEffect.Play();
    }

    private void Update()
    {
        if (particleSystemEffect.IsAlive())
            return;

        ObjectPoolManager.Instance.Return(gameObject);
    }
}