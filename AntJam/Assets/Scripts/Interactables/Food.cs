using DG.Tweening;
using UnityEngine;

public class Food : Interactable
{
    [SerializeField] private int foodValue = 1;
    
    public override void Interact()
    {
        PlayerManager.Instance.ActivateParticles();
        AudioManager.Instance.Play("Eat");
        PlayerAnt.Instance.AddFood(foodValue);
        CollectAnim(PlayerManager.Instance.foodCool.position);
    }
    
}
