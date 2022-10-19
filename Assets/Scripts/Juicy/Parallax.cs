using System;
using DG.Tweening;
using UnityEngine;

namespace Juicy
{
    public class Parallax : MonoBehaviour
    {
        public GameObject Player;
        public float Speed;
        public float MotionMult;
        
        private float height;
        
        private void Start()
        {
            height = GetComponent<BoxCollider2D>().size.y;
            DoAnim();
        }

        private void Update()
        {
            if (Player == null) return;
            float tX = -Player.transform.position.x * MotionMult;
            transform.position = new Vector2(tX, transform.position.y);
        }

        private void DoAnim()
        {
            transform.DOMoveY(transform.position.y - height, Speed)
                .SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}