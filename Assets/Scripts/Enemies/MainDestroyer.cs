using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveX(transform.position.x + 30, 10).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOMoveY(transform.position.y + 2, 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
