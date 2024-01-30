using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public enum WeaponTypes
    {
        Nothing,
        Blaster,
        Spread,
        ShotGun,
        Laser
    }
    
    public class WeaponManager : MonoBehaviour
    {
        public List<WeaponStats> weaponsData;

        public static WeaponManager s;
        public static readonly int WEAPON_MAX_LEVEL = 5;

        public static readonly Dictionary<WeaponTypes, Type> Weapons = new Dictionary<WeaponTypes, Type>()
        {
            { WeaponTypes.Blaster, typeof(BlasterWeapon) },
            { WeaponTypes.Spread, typeof(SpreadWeapon) },
            { WeaponTypes.ShotGun, typeof(ShotGunWeapon) },
            { WeaponTypes.Laser, typeof(LaserWeapon) }
        };

        public static readonly List<WeaponTypes> WeaponsChance = new List<WeaponTypes>()
        {
            WeaponTypes.Blaster,
            WeaponTypes.Blaster,
            WeaponTypes.Blaster,
            WeaponTypes.Spread,
            WeaponTypes.Spread,
            WeaponTypes.ShotGun,
            WeaponTypes.ShotGun,
            //WeaponTypes.Laser
        };
        
        private Dictionary<string, WeaponStats> weaponsDataMap;

        private void Awake()
        {
            if (s != null)
            {
                Debug.LogError("WeaponData already exist");
                return;
            }
            
            s = this;

            weaponsDataMap = new Dictionary<string, WeaponStats>();
            foreach (var stat in weaponsData)
            {
                weaponsDataMap.Add(stat.weaponTypeName, stat);
            }
            
            // Проверим, что покрыли все типы в описании
            var type = typeof(IWeapon);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            
            foreach (var t in types)
            {
                if (string.Compare(t.Name, nameof(IWeapon), StringComparison.Ordinal) == 0)
                {
                    continue;
                }

                weaponsDataMap.TryGetValue(t.Name, out var weapon);

                if (weapon == null)
                {
                    Debug.LogError("Type " + t.Name + " isn't covered in default value definition!");
                }
            }
        }

        public GameObject CreateBullet(GameObject bullet, Vector2 position)
        {
            return Instantiate(bullet, position, Quaternion.identity);
        }

        public WeaponStats GetStat<TWeapon>(TWeapon weapon) where TWeapon : IWeapon
        {
            return weaponsDataMap[weapon.GetType().Name];
        }
    }
}
