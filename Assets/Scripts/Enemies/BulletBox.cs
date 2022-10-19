using System.Collections.Generic;
using DG.Tweening;
using Interactions;
using UnityEngine;

namespace Enemies
{
    public class BulletBox : MonoBehaviour
    {
        public GameObject Bullet;
        public float FireForce;
        public float Damage = 1;
        
        private readonly List<Vector2> directions = new List<Vector2>()
        {
            new Vector2(0, 1),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
            new Vector2(1, 1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, -1).normalized
        };

        private void EmitBullets(Quaternion angle)
        {
            foreach (var dir in directions)
            {
                var tmp = Instantiate(Bullet, transform.position, Quaternion.identity);
                tmp.GetComponent<Bullet>().Launch(FireForce, angle * dir, Damage);
            }
        }
        
        private void Start()
        {
            var destroyer = GetComponent<Destroyable>();
            
            transform.parent.GetComponent<Destroyable>().OnDestroyStart += () => { destroyer.InvokeDestroy(); };
            destroyer.OnDestroyStart += () =>
            {
                var sequence = DOTween.Sequence();
                sequence
                    .AppendCallback(() => EmitBullets(Quaternion.Euler(0, 0, 0f)))
                    .AppendInterval(0.2f)
                    .AppendCallback(() => EmitBullets(Quaternion.Euler(0, 0, 15f)))
                    .AppendInterval(0.2f)
                    .AppendCallback(() => EmitBullets(Quaternion.Euler(0, 0, 30f)));
            };
        }
    }
}
