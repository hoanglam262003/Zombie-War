using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSFX : MonoBehaviour
{
    [Header("Shoot")]
    [SerializeField]
    private AudioClip shootSound;

    [Header("Throw")]
    [SerializeField]
    private AudioClip throwSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
    }

    public void PlayShoot()
    {
        if (shootSound == null)
            return;

        audioSource.PlayOneShot(shootSound);
    }

    public void PlayThrow()
    {
        if (throwSound == null)
            return;

        audioSource.PlayOneShot(throwSound);
    }
}