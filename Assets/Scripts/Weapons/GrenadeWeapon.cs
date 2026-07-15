using UnityEngine;

public class GrenadeWeapon : WeaponBehaviour
{
    [Header("Config")]
    [SerializeField]
    private WeaponConfig config;

    [SerializeField]
    private GameObject grenadeThrowPrefab;
    [SerializeField]
    private float throwForce = 12f;

    [SerializeField]
    private float throwUpForce = 2f;

    [Header("References")]
    [SerializeField]
    private Transform throwPoint;

    [SerializeField]
    private GameObject handGrenadeVisual;

    private float nextThrowTime;

    public override void BeginAim()
    {
        base.BeginAim();
    }

    public override void EndAim()
    {
        base.EndAim();
    }

    public override void Attack()
    {
        if (Time.time < nextThrowTime)
            return;

        nextThrowTime =
            Time.time + 1f / config.fireRate;
        PlayThrowSound();
        PlayThrowAnimation();
    }

    public override void BeginThrow()
    {
        if (handGrenadeVisual != null)
            handGrenadeVisual.SetActive(false);
        ThrowGrenade();
    }

    public override void EndThrow()
    {
        if (handGrenadeVisual != null)
            handGrenadeVisual.SetActive(true);
    }

    private void ThrowGrenade()
    {
        GameObject grenade =
            ObjectPoolManager.Instance.Get(
                PoolType.Grenade,
                throwPoint.position,
                Quaternion.identity);

        if (grenade == null)
            return;

        GrenadeBehaviour behaviour =
            grenade.GetComponent<GrenadeBehaviour>();

        if (behaviour == null)
            return;

        Vector3 direction =
            playerController.Movement.FacingDirection;

        direction.y = 0f;
        direction.Normalize();

        Vector3 velocity =
            direction * throwForce +
            Vector3.up * throwUpForce;

        behaviour.Throw(velocity);
    }
}