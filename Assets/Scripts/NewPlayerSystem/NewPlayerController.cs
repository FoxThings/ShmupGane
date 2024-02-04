using System;
using System.Collections.Generic;
using Assets.Scripts.NewPlayerSystem.Devices.HelpDevices;
using Assets.Scripts.NewPlayerSystem.Devices.Shields;
using DG.Tweening;
using Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapons;

namespace Assets.Scripts.NewPlayerSystem
{
    public class NewPlayerController : MonoBehaviour
    {
        public GameObject Shield1;
        public GameObject Shield2;
        
        public float maxVelocity;
        public float timeToMaxVelocity;
        public float timeToZeroVelocity;
        public float upwardDragModifier;

        private Vector2 axes;

        private Rigidbody2D body;
        private Animator animator;

        private static readonly int Movement = Animator.StringToHash("Movement");

        private ShipModel shipModel;

        public ShipModel GetModel()
        {
            return shipModel;
        }
        
        private void Start()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

            GetComponent<Destroyable>().OnDestroyFinish += () =>
            {
                var sequence = DOTween.Sequence();
                sequence.AppendInterval(1f);
                sequence.AppendCallback(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            };

            shipModel = new ShipModel();
            var result = shipModel.AddDevice(5, new SpreadW());
            result &= shipModel.AddDevice(4, new LocatorD());
            result &= shipModel.AddDevice(1, new Shield(Shield1));

            if (!result)
            {
                Debug.LogError("First weapon wasn't attached");
            }
        }

        private void Update()
        {
            axes = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            animator.SetFloat(Movement, axes.x);
        }

        private void FixedUpdate()
        {
            // Берем разницу до максимальной скорости
            var targetVelocities = axes * maxVelocity - body.velocity;

            // Ускоряемся быстрее, а замедляемся медленнее!
            targetVelocities /= Mathf.Approximately(axes.magnitude, 0f) ? timeToZeroVelocity : timeToMaxVelocity;

            // Если мы движемся вперед, то это даётся нам труднее
            targetVelocities *= axes.y > 0f ? upwardDragModifier : 1;

            // Применяем скорость
            body.AddForce(targetVelocities, ForceMode2D.Force);
        }
    }
}
