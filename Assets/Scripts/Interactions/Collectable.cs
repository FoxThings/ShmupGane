using System;
using DG.Tweening;
using UI;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Interactions
{
    public enum CollectableType
    {
        Upgrade,
        Weapon,
        Health
    }
    public class Collectable : MonoBehaviour
    {
        private void Start()
        {
            transform.DOMoveY(transform.position.y - 5, 8).SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            transform.DOMoveX(transform.position.x + 0.5f, 1).SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
            transform.DOScale(transform.localScale / 1.4f, 1).SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutExpo).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public CollectableType Type;

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (Type)
            {
                case CollectableType.Upgrade:
                    WeaponPanel.S.DrawPanelUpgrade();
                    break;
                case CollectableType.Weapon:
                    var list = WeaponManager.WeaponsChance;
                    WeaponPanel.S.DrawPanelWeapon(list[Random.Range(0, list.Count)]);
                    break;
                case CollectableType.Health:
                    other.gameObject.TryGetComponent<Damageable>(out var dmg);
                    if (dmg != null)
                    {
                        dmg.DoDamage(-1);
                    };
                    break;
            }
            Destroy(gameObject);
        }
    }
}