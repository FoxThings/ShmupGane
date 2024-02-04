using UnityEngine;
using Weapons;

public class BlasterW : Weapon
{
    public BlasterW()
    {
        description = "Пулемет\nТратит 0.5 энергии на пулю, превосходная точность";
    }
    
    protected override float getChargesToFire()
    {
        return 0.5f;
    }
    
    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {
        GameObject bullet = WeaponManager.s.CreateBullet(bulletObj, startPoint);
        bullet.GetComponent<Bullet>()?.Launch(force, direction);
    }
}
