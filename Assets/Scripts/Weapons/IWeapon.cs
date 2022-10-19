using DG.Tweening;
using UnityEngine;

namespace Weapons
{
    public abstract class IWeapon : MonoBehaviour
    {
        protected WeaponLevel stat;
        protected bool canShoot = true; 
        protected GameObject bulletPrefab;

        private int level = 1;
        
        protected void UpdateCurrentStats()
        {
            stat = WeaponManager.s.GetStat(this).levels[level - 1];
            bulletPrefab = WeaponManager.s.GetStat(this).bulletPrefab;
        }

        public void Upgrade()
        {
            Debug.Log("Upgraded");
            if (level == WeaponManager.WEAPON_MAX_LEVEL) return;

            ++level;
            UpdateCurrentStats();
        }

        protected virtual void Start()
        {
            UpdateCurrentStats();
        }

        protected virtual void Update()
        {
            ManageInput();
        }

        protected virtual void ManageInput()
        {
            if (Input.GetButton("Jump") && canShoot)
            {
                Shoot(gameObject.transform.position, Vector2.up, stat.force);
                canShoot = false;

                var sequence = DOTween.Sequence();
                sequence.AppendInterval(stat.fireRate).OnComplete(() => canShoot = true);
            }
        }

        protected virtual void Shoot(Vector2 startPoint, Vector2 direction, float force)
        {
            GameObject bullet = WeaponManager.s.CreateBullet(bulletPrefab, startPoint);
            bullet.GetComponent<Bullet>()?.Launch(force, direction, stat.damage);
        }
    }
}