using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected Collider Coll;

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

    public abstract void Interact();
}
