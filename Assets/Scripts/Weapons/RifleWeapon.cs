using UnityEngine;

public class RifleWeapon : WeaponBehaviour
{
    public override void Attack()
    {
        Debug.Log("Rifle Fire");
        PlayShootAnimation();
    }

    public override void BeginAim()
    {
        Debug.Log("Aim Rifle");
        base.BeginAim();
    }

    public override void EndAim()
    {
        Debug.Log("Stop Aim Rifle");
        base.EndAim();
    }
}