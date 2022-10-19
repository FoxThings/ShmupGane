using System;
using UnityEngine;

namespace Interactions
{
    public class Damaging : MonoBehaviour
    {
        public float Damage;
        public Action OnDamage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.TryGetComponent<Damageable>(out var damg)) return;

            damg.DoDamage(Damage);
            OnDamage?.Invoke();
        }
    }
}
