using DG.Tweening;
using UnityEngine;

public class Secret : Interactable
{
    public override void Interact()
    {
        PlayerManager.Instance.AquireSecret();
        CollectAnim(PlayerManager.Instance.secretCool.position);
    }
}
