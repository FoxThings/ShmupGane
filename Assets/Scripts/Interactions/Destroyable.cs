using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Interactions
{
    public class Destroyable : MonoBehaviour
    {
        public List<GameObject> ToDelete;
        public Action OnDestroyFinish;
        public Action OnDestroyStart;
        
        private static readonly int Death = Animator.StringToHash("Death");

        private Sequence sequence;

        private void DestroyHandler(float health)
        {
            if (health > 0f) return;

            OnDestroyStart?.Invoke();

            if (TryGetComponent<BoxCollider2D>(out var coll))
            {
                coll.enabled = false;
            }

            var animator = GetComponent<Animator>();
            animator.SetTrigger(Death);

            var animController = animator.runtimeAnimatorController;
            var clip = animController.animationClips.First(a => a.name == "Death");
            var length = clip.length;

            foreach (var obj in ToDelete)
            {
                obj.GetComponent<SpriteRenderer>().DOFade(0, length - 0.05f);
            }
            
            sequence = DOTween.Sequence();
            sequence.AppendInterval(length)
                .OnComplete(() =>
                {
                    OnDestroyFinish?.Invoke();
                    Destroy(gameObject);
                })
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        public void InvokeDestroy()
        {
            DestroyHandler(0);
        }
        
        private void Start()
        {
            if (TryGetComponent<Damageable>(out var damageable))
            {
                damageable.OnChangeHealth += DestroyHandler;
            }
        }
    }
}