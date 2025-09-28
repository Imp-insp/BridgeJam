using System;
using DG.Tweening;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [Header("Variables")] public static float moveTime => 0.5f;


    [Header("Ref")] public Transform end;

    [HideInInspector] public SpriteRenderer sprRenderer;
    private PlayerAnt _playerAnt;


    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _playerAnt = PlayerAnt.Instance;
        
        DeactivateColl();
        sprRenderer.enabled = false;
    }

    public void Activate(Vector3 groundTrgt, Vector3 lastTrgt)
    {
        
        sprRenderer.enabled = true;

        transform.DOMove(groundTrgt, moveTime).OnComplete(() =>
        {
            Debug.Log(Vector2.SignedAngle(transform.position, lastTrgt));
            transform.LookAt(lastTrgt);
            _playerAnt.areWalking = false;
            SetLayerRecursively(gameObject, LayerMask.NameToLayer("Wall"));
        });
    }

    public void Activate(Vector3 groundTrgt, Vector3 lastTrgt, Vector3 nextPoint)
    {
        sprRenderer.enabled = true;

        transform.DOMove(groundTrgt, moveTime).OnComplete(() =>
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z + Vector2.SignedAngle(transform.position, lastTrgt));
            transform.DOMove(nextPoint, moveTime).OnComplete(() =>
            {
                _playerAnt.areWalking = false;
                SetLayerRecursively(gameObject, LayerMask.NameToLayer("Wall"));
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