using DG.Tweening;
using UnityEngine;

public class Secret : Interactable
{
    public override void Interact()
    {
        PlayerManager.Instance.AquireSecret();
        transform.DOKill();
        Destroy(gameObject.transform.parent.gameObject);
    }
}
