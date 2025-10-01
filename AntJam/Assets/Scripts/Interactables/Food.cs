using DG.Tweening;
using UnityEngine;

public class Food : Interactable
{
    [SerializeField] private int foodValue = 1;
    
    public override void Interact()
    {
        PlayerAnt.Instance.AddFood(foodValue);
        transform.DOKill();
        Destroy(gameObject);
        
    }
    
}
