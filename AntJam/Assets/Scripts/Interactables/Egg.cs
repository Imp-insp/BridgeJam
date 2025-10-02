using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Egg : Interactable
{
    [Header("Tramble")] [SerializeField] private float trambleOffSet;
    [SerializeField] private int trambleTimes;
    [SerializeField] private float trambleCd;
    [Header("Shake")] [SerializeField] private float shakeOffset;
    [SerializeField] private float animTime;


    [Header("Control")] private int _wiggleAmount;

    private void Start()
    {
        StartCoroutine(Wiggle());
    }

    public override void Interact()
    {
        PlayerAnt.Instance.AddAnt();
        transform.DOKill();
        Destroy(gameObject.transform.parent.gameObject);
    }

    private IEnumerator Wiggle()
    {
        if (_wiggleAmount == 3)
        {
            
            for (var i = 0; i < trambleTimes; i++)
            {
                var targetTramble = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
                    transform.eulerAngles.z - trambleOffSet);
                transform.DORotate(targetTramble, trambleCd / trambleTimes).SetLoops(2, LoopType.Yoyo);
                trambleOffSet = -trambleOffSet;
                yield return new WaitForSeconds(trambleCd / trambleTimes *2);
            }
            
            _wiggleAmount = 0;
        }

        var targetRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y,
            transform.eulerAngles.z + shakeOffset);
        transform.DORotate(targetRotation, animTime).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            _wiggleAmount++;
            shakeOffset = -shakeOffset;
            StartCoroutine(Wiggle());
        });
    }
}