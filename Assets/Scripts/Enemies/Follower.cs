using DG.Tweening;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject Next;
    public float Time;

    private void Move()
    {
        transform.DOMove(Next.transform.position, Time).OnComplete(Move).SetEase(Ease.Linear);
    }
    void Start()
    {
        transform.DOLookAt(Next.transform.position, 200);
        Move();
    }
    
}
