using DG.Tweening;
using Interactions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    internal enum InvaderStates
    {
        Move,
        Attack
    }

    public class Invader : MonoBehaviour
    {
        public float Damage = 1;
        public Vector2 Direction;
        public float Speed;
        public float FireDelay;
        public float FireForce;
        public GameObject Bullet;

        private InvaderStates state;

        private Tween currentTween;
        private Sequence currentSequence;
        private BoundsCheck bounds;

        private void ChangeState(InvaderStates newState)
        {
            state = newState;
            StateManager();
        }

        private void StateManager()
        {
            switch (state)
            {
                case InvaderStates.Move:
                    MoveBehaviour();
                    break;

                case InvaderStates.Attack:
                    AttackBehaviour();
                    break;
            }
        }

        private void MoveBehaviour()
        {
            var possibleDirection = new Vector2(
                Random.Range(-Direction.x, Direction.x), 
                Random.Range(-Direction.y / 2, -Direction.y));
            
            if (bounds.OffUp) possibleDirection = new Vector2(0f, -2f);
            if (Vector2.Distance(transform.position, new Vector2(-bounds.Width, transform.position.y)) < Direction.x) 
                possibleDirection.x = Direction.x;
            if (Vector2.Distance(transform.position, new Vector2(bounds.Width, transform.position.y)) < Direction.x) 
                possibleDirection.x = -Direction.x;
            if (bounds.OffDown && 
                Vector2.Distance(transform.position, new Vector2(transform.position.x, -bounds.Height)) > 2)
            {
                GetComponent<Destroyable>().InvokeDestroy();
                return;
            }

            currentTween = transform.DOMove(
                (Vector2)transform.position + possibleDirection,
                Speed
            ).SetEase(Ease.InOutExpo).OnComplete(() => ChangeState(InvaderStates.Attack));

        }

        private void AttackBehaviour()
        {
            void BulletCreation()
            {
                var tmp = Instantiate(Bullet, transform.position, Quaternion.identity);
                tmp.GetComponent<Bullet>().Launch(FireForce, Vector2.down, Damage);
            }

            currentSequence = DOTween.Sequence();
            currentSequence.AppendCallback(BulletCreation)
                .AppendInterval(FireDelay)
                .SetLoops(3)
                .SetEase(Ease.Linear)
                .OnComplete(() => ChangeState(InvaderStates.Move));
        }

        private void Stop()
        {
            currentSequence.Kill();
            currentTween.Kill();
        }

        private void Start()
        {
            if (TryGetComponent<Destroyable>(out var destroyer))
            {
                destroyer.OnDestroyStart += Stop;
            }

            bounds = GetComponent<BoundsCheck>();
            
            state = InvaderStates.Move;
            StateManager();
        }
        
    }
}
