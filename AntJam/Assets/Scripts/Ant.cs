using System;
using DG.Tweening;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [Header("Variables")] public static float moveTime => 0.5f;


    [Header("Ref")] public Transform end;
    private BoxCollider2D _boxCollider2D;
    [HideInInspector] public SpriteRenderer sprRenderer;
    private PlayerAnt _playerAnt;


    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
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
            Vector2 direction = InputHandler.mousePos - (Vector2) transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.DORotateQuaternion(targetRotation, moveTime).OnComplete(() =>
            {
                Debug.Log(angle);
                _playerAnt.areWalking = false;
                _boxCollider2D.enabled = true;
            });;
            
        });
    }

    public void Activate(Vector3 groundTrgt, Vector3 lastTrgt, Vector3 nextPoint)
    {
        sprRenderer.enabled = true;

        transform.DOMove(groundTrgt, moveTime).OnComplete(() =>
        {
            Vector2 direction = InputHandler.mousePos -(Vector2) transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.DORotateQuaternion(targetRotation, moveTime).OnComplete(() =>
            {
                transform.DOMove(nextPoint, moveTime).OnComplete(() =>
                {
                    _playerAnt.areWalking = false;
                    _boxCollider2D.enabled = true;
                });
            });
        });
    }


    public void DeactivateColl()
    {
        _boxCollider2D.enabled = false;
    }

    /*public void PutCol(bool first)
    {
        var cool = first
            ? gameObject.AddComponent<CircleCollider2D>()
            : end.gameObject.AddComponent<CircleCollider2D>();
        cool.radius = 0.4f;
    }
    */
}