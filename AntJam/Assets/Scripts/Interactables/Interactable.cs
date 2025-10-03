using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private static float MoveTime => 0.5f;
    
    
    protected Collider Coll;
    protected bool CanDie;

    private void Awake()
    {
        Coll = GetComponent<Collider>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Interact();
        }
    }
    protected void CollectAnim(Vector2 newPosition)
    {
        transform.DOKill();
        transform.DOMove(newPosition, MoveTime).OnComplete(() =>
        {
            transform.DOKill();
            Destroy(gameObject.transform.parent.gameObject);    
        });
        
    }

    public abstract void Interact();
}
