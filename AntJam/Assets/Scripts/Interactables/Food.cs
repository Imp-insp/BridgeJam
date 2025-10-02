using DG.Tweening;
using UnityEngine;

public class Food : Interactable
{
    [SerializeField] private int foodValue = 1;
    
    public override void Interact()
    {
        AudioManager.Instance.Play("Eat");
        PlayerAnt.Instance.AddFood(foodValue);
        transform.DOKill();
        Destroy(gameObject.transform.parent.gameObject);
    }
    
}
