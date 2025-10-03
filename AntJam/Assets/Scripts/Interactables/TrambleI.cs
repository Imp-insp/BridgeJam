using System;
using DG.Tweening;
using UnityEngine;

public class TrambleI : MonoBehaviour
{
    [Header("Control")] [SerializeField] private bool shouldSquish = true;
    [SerializeField] private bool shouldTremble = true;
    
    [Header("Animation")] [SerializeField] private float squishOffSet;
    [SerializeField] private float squishCd;
    [SerializeField] private float trambleOffSet;
    [SerializeField] private float trambleCd;

    private void Start()
    {
        Tremble();
    }

    private void Tremble()
    {
        if (shouldSquish)
        {
            var targetSquish = new Vector3(transform.localScale.x + squishOffSet, transform.localScale.y + squishOffSet,
                transform.localScale.z);
            transform.localScale = new Vector3(transform.localScale.x - squishOffSet,
                transform.localScale.y - squishOffSet,
                transform.localScale.z);
            transform.DOScale(targetSquish, squishCd).SetLoops(-1, LoopType.Yoyo);
        }

        if (shouldTremble)
        {
            var trans = transform.localEulerAngles;
            var targetTramble = new Vector3(trans.x, trans.y,
                trans.z + trambleOffSet);
            transform.localEulerAngles = new Vector3(trans.x, trans.y,
                trans.z - trambleOffSet);
            transform.DORotate(targetTramble, trambleCd).SetLoops(-1, LoopType.Yoyo);
        }
    }
}
