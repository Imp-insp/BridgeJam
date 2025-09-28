using System;
using DG.Tweening;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [Header("Variables")] 
    [SerializeField] private float moveTime;


    [Header("Ref")] 
    public Transform end;

    [HideInInspector] public SpriteRenderer sprRenderer;


    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        DeactivateColl();
        sprRenderer.enabled = false;
    }
    
    public void Activate(Vector3 groundTrgt,Vector3 lastTrgt)
    {
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Wall"));

        transform.DOMove(groundTrgt, moveTime).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, Vector2.SignedAngle(transform.position, lastTrgt)), moveTime);
        });

    }
    public void Activate(Vector3 groundTrgt, Vector3 lastTrgt, Vector3 nextPoint)
    {
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Wall"));
       
        transform.DOMove(groundTrgt, moveTime).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, Vector2.SignedAngle(transform.position, lastTrgt)), moveTime).OnComplete(() =>
            {
                transform.DOMove(nextPoint, moveTime);
            });
        });

    }


    public void DeactivateColl()
    {
        SetLayerRecursively(gameObject, LayerMask.NameToLayer("Default"));
        
    }

    /*public void PutCol(bool first)
    {
        var cool = first
            ? gameObject.AddComponent<CircleCollider2D>()
            : end.gameObject.AddComponent<CircleCollider2D>();
        cool.radius = 0.4f;
    }
    */

    public void GoToPlace(Vector3 position)
    {
        //_rb2D.linearVelocity
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}