using UnityEngine;

public class GrenadeWeapon : WeaponBehaviour
{
    public override void Attack()
    {
        Debug.Log("Throw Grenade");
        PlayThrowAnimation();
    }

    public override void BeginAim()
    {
        base.BeginAim();
        Debug.Log("Preview Throw");
    }

    public override void EndAim()
    {
        base.EndAim();
        Debug.Log("Cancel Throw");
    }
}