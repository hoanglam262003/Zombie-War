using UnityEngine;

public class BloodEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystemEffect;

    private void Awake()
    {
        if (particleSystemEffect == null)
            particleSystemEffect = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        particleSystemEffect.Play();
    }

    private void Update()
    {
        if (particleSystemEffect == null)
            return;

        if (particleSystemEffect.IsAlive())
            return;

        ObjectPoolManager.Instance.Return(gameObject);
    }
}