using UnityEngine;
using Weapons;

public class ShotGunWeapon : IWeapon
{
    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {

        for (int i = 0; i < stat.bulletsCount; ++i)
        {
            var angle = Quaternion.Euler(0, 0, Random.Range(-45f, 45f));
            base.Shoot(startPoint, angle * direction, Random.Range(force / 2, force));
        }
    }
}
