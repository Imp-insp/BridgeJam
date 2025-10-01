using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Ant : MonoBehaviour
{
    [Header("Ref")] public Transform end;
    private BoxCollider2D _boxCollider2D;
    [HideInInspector] public SpriteRenderer sprRenderer;
    private PlayerAnt _playerAnt;

    [Header("Animation")] [SerializeField] private float squishOffSet;
    [SerializeField] private float squishCd;
    [SerializeField] private float trambleOffSet;
    [SerializeField] private float trambleCd;


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

    public void Activate(Vector2 direction)
    {
        sprRenderer.enabled = true;
        var moveTime = PlayerAnt.Instance.antMoveTime;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.DORotateQuaternion(targetRotation, moveTime).OnComplete(() =>
        {
            Tremble();
            _playerAnt.areWalking = false;
            _boxCollider2D.enabled = true;
        });
        ;
    }

    public void Activate(Vector3 nextPoint, Vector2 direction)
    {
        sprRenderer.enabled = true;
        var moveTime = PlayerAnt.Instance.antMoveTime;
        

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.DORotateQuaternion(targetRotation, moveTime).OnComplete(() =>
        {
            Tremble();
            transform.DOMove(nextPoint, moveTime).OnComplete(() =>
            {
                _playerAnt.areWalking = false;
                _boxCollider2D.enabled = true;
            });
        });
    }


    public void DeactivateColl()
    {
        _boxCollider2D.enabled = false;
        sprRenderer.enabled = false;

        transform.DOKill();
    }

    private void Tremble()
    {
        var targetSquish = new Vector3(transform.localScale.x + squishOffSet, transform.localScale.y,
            transform.localScale.z);
        transform.localScale = new Vector3(transform.localScale.x - squishOffSet, transform.localScale.y,
            transform.localScale.z);
        transform.DOScale(targetSquish, squishCd).SetLoops(-1, LoopType.Yoyo);

        
        var trans = transform.localEulerAngles;
        var targetTramble = new Vector3(trans.x, trans.y,
            trans.z+ trambleOffSet);
        transform.localEulerAngles = new Vector3(trans.x , trans.y,
            trans.z- trambleOffSet);
        transform.DORotate(targetTramble, trambleCd).SetLoops(-1, LoopType.Yoyo);
    }

    
    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            PlayerAnt.hitWall = true;
        }
    }*/

    /*public void PutCol(bool first)
    {
        var cool = first
            ? gameObject.AddComponent<CircleCollider2D>()
            : end.gameObject.AddComponent<CircleCollider2D>();
        cool.radius = 0.4f;
    }
    */
}