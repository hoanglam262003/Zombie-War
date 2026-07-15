using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    [SerializeField]
    private WeaponVFX weaponVFX;
    [SerializeField]
    private WeaponSFX weaponSFX;

    protected PlayerController playerController;

    public virtual void Initialize(PlayerController controller)
    {
        playerController = controller;
    }

    public abstract void Attack();
    protected void PlayMuzzleFlash()
    {
        if (weaponVFX != null)
            weaponVFX.PlayMuzzleFlash();
    }
    protected void PlayShootAnimation()
    {
        playerController.Animation.PlayShoot();
    }
    protected void BeginFireAnimation()
    {
        playerController.Animation.BeginFire();
    }
    protected void EndFireAnimation()
    {
        playerController.Animation.EndFire();
    }
    protected void PlayThrowAnimation()
    {
        playerController.Animation.PlayThrow();
    }
    public virtual void BeginAim()
    {
        playerController.Animation.SetAim(true);
    }

    public virtual void EndAim()
    {
        playerController.Animation.SetAim(false);
    }
    public virtual void BeginFire()
    {
    }

    public virtual void EndFire()
    {
    }

    public virtual void BeginThrow()
    {
    }

    public virtual void EndThrow()
    {
    }

    protected void PlayShootSound()
    {
        if (weaponSFX != null)
            weaponSFX.PlayShoot();
    }

    protected void PlayThrowSound()
    {
        if (weaponSFX != null)
            weaponSFX.PlayThrow();
    }
    protected void SpawnBullet(WeaponConfig config, Transform firePoint)
    {
        GameObject bulletObj =
            ObjectPoolManager.Instance.Get(
                config.bulletPool,
                firePoint.position,
                firePoint.rotation);

        if (bulletObj == null)
            return;

        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if (bullet == null)
            return;

        Vector3 direction = playerController.Movement.FacingDirection;

        bullet.Initialize(
            config.damage,
            config.speed,
            direction);
    }
}