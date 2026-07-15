using UnityEngine;

public class WeaponVFX : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem muzzleFlash;

    public void PlayMuzzleFlash()
    {
        if (muzzleFlash == null)
            return;

        muzzleFlash.Stop(
            true,
            ParticleSystemStopBehavior.StopEmittingAndClear);

        muzzleFlash.Play();
    }
}