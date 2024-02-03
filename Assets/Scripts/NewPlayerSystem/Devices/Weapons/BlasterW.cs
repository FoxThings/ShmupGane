using UnityEngine;
using Weapons;

public class BlasterW : Weapon
{
    protected override float getChargesToFire()
    {
        return 0.5f;
    }
    
    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {
        GameObject bullet = WeaponManager.s.CreateBullet(Resources.Load("StandardBullet") as GameObject, startPoint);
        bullet.GetComponent<Bullet>()?.Launch(force, direction, 0.25f);
    }
}
