using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Interactions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Weapons;

public class LaserWeapon : IWeapon
{
    private GameObject laser;
    private Collider2D cl;
    private Damageable dmg;

    protected override void Start()
    {
        base.Start();
        laser = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity, transform);
        cl = laser.GetComponent<Collider2D>();
        cl.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetButtonUp("Jump"))
        {
            
        }
        if (!canShoot)
        {
            cl.enabled = false;
        }
    }

    protected override void Shoot(Vector2 startPoint, Vector2 direction, float force)
    {
        cl.enabled = true;
    }
}
