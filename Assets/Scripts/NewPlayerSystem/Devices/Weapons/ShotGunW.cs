using UnityEngine;
using Weapons;

public class ShotGunW : BlasterW
{
    protected override float getChargesToFire()
    {
        return 8f;
    }
    
    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {
        for (int i = 0; i < 20; ++i)
        {
            var angle = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
            base.Shoot(startPoint, angle * direction, Random.Range(force / 2f, force / 1.5f));
        }
    }
}
