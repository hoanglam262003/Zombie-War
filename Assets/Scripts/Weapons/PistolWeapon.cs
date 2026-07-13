using UnityEngine;

public class PistolWeapon : WeaponBehaviour
{
    public override void Attack()
    {
        Debug.Log("Pistol Fire");
        PlayShootAnimation();
    }

    public override void BeginAim()
    {
        base.BeginAim();
        Debug.Log("Aim Pistol");
    }

    public override void EndAim()
    {
        base.EndAim();
        Debug.Log("Stop Aim Pistol");
    }
}