using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class SpreadWeapon : IWeapon
{
    private readonly List<Quaternion> angles = new List<Quaternion>() { 
        Quaternion.Euler(0, 0, 0f),
        Quaternion.Euler(0, 0, -10f),
        Quaternion.Euler(0, 0, 10f),
        Quaternion.Euler(0, 0, -20f),
        Quaternion.Euler(0, 0, 20f),
        Quaternion.Euler(0, 0, -30f),
        Quaternion.Euler(0, 0, 30f)
    };

    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {

        for (int i = 0; i < stat.bulletsCount; ++i)
        {
            base.Shoot(startPoint, angles[i] * direction, force);
        }
    }
}
