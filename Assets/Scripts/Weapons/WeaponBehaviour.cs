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
}