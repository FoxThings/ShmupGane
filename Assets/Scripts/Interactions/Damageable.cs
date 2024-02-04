using System;
using DG.Tweening;
using UnityEngine;

namespace Interactions
{
    public class Damageable : MonoBehaviour
    {
        public float MaxHealth;
        public float FlashDuration;
        public float InvisibleTime;

        private float health;
        private bool isVisible = true;

        private bool canDamaged = true;
        
        private Material material;
        private Tween flashTween;
        
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
        
        public delegate void ChangeHealthHandler(float health);
        
        public ChangeHealthHandler OnChangeHealth;

        private float Flash
        {
            get => material.GetFloat(FlashAmount);
            set => material.SetFloat(FlashAmount, value);
        }

        protected virtual void Start()
        {
            health = MaxHealth;
            material = GetComponent<SpriteRenderer>().material;
            
            if (TryGetComponent<Destroyable>(out var destroyer))
            {
                destroyer.OnDestroyStart += () =>
                {
                    canDamaged = false;
                };
            }
        }
        
        private void DoFlash()
        {
            flashTween.Complete();

            Flash = 1;
            flashTween = DOTween.To(() => Flash, x => Flash = x, 0, FlashDuration)
                .SetEase(Ease.OutSine)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        protected virtual void ChangeHealth(float damage)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, MaxHealth);
            OnChangeHealth?.Invoke(health);
            
        }

        public void DoDamage(float damage)
        {
            if (!canDamaged) return;
            if (!isVisible) return;
            
            DoFlash();
            ChangeHealth(damage);

            isVisible = false;
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(InvisibleTime).OnComplete(() => isVisible = true);
        }
    }
}
