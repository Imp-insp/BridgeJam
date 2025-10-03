using DG.Tweening;
using UnityEngine;

public class Secret : Interactable
{
    public override void Interact()
    {
        AudioManager.Instance.Play("Secret");
        PlayerManager.Instance.ActivateParticles();
        PlayerManager.Instance.AquireSecret();
        CollectAnim(PlayerManager.Instance.secretCool.position);
        
    }
}
