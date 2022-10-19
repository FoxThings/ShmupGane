using System;
using System.Collections.Generic;
using UnityEngine;

namespace Weapons
{
    [Serializable]
    public class WeaponStats
    {
        public string weaponTypeName;
        public GameObject bulletPrefab;
        public List<WeaponLevel> levels;
    }

    [Serializable]
    public class WeaponLevel
    {
        public float fireRate = 0.5f;
        public float damage = 1;
        public float force = 1000;
        public int bulletsCount = 1;
    }
}  