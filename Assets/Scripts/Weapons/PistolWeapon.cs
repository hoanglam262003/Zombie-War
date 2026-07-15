using UnityEngine;

public class PistolWeapon : WeaponBehaviour
{
    [SerializeField]
    private WeaponConfig config;

    [SerializeField]
    private Transform firePoint;

    private float nextFireTime;

    public override void Attack()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime =
            Time.time + 1f / config.fireRate;

        SpawnBullet(config, firePoint);
        PlayMuzzleFlash();
        PlayShootSound();
        PlayShootAnimation();
    }
}