using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scriptable Objects/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public int damage = 20;

    public float fireRate = 2f;

    public float speed = 40f;

    public PoolType bulletPool = PoolType.Bullet;

    public LayerMask hitMask;
}
