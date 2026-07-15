using UnityEngine;

public abstract class WeaponBehaviour : MonoBehaviour
{
    protected PlayerController playerController;

    public virtual void Initialize(PlayerController controller)
    {
        playerController = controller;
    }

    public abstract void Attack();
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
            config.bulletSpeed,
            direction);
    }
}