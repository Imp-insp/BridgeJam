using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Header("WallCheck")] [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundMask;

    [Header("BugFix")] private Vector2 _originalScale;

    [Header("WallCheck")] private int _gossipNumber;
    private void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        _boxCollider2D = GetComponent<BoxCollider2D>();

        _originalScale = transform.localScale;
    }

    private void Start()
    {
        _playerAnt = PlayerAnt.Instance;

        DeactivateColl();
        sprRenderer.enabled = false;
    }

    public void Activate(Vector2 direction)
    {
        _gossipNumber = Random.Range(0, 4);
        AudioManager.Instance.Play("Gossip " + _gossipNumber);
        sprRenderer.enabled = true;
        _boxCollider2D.enabled = true;
        var moveTime = PlayerAnt.Instance.antMoveTime;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.DORotateQuaternion(targetRotation, moveTime).OnComplete(() =>
        {
            Tremble();
            CheckForWall();
            _playerAnt.areWalking = false;
        });
        ;
    }

    public void Activate(Vector3 nextPoint, Vector2 direction, int index)
    {
        _gossipNumber = Random.Range(0, 4);
        AudioManager.Instance.Play("Gossip " + _gossipNumber);
        
        sprRenderer.enabled = true;
        var moveTime =  PlayerAnt.Instance.antMoveTime ;
        _boxCollider2D.enabled = true;
        

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.DORotateQuaternion(targetRotation, moveTime/index).OnComplete(() =>
        {
            Tremble();
            transform.DOMove(nextPoint, moveTime).OnComplete(() =>
            {
                CheckForWall();
                _playerAnt.areWalking = false;
            });
        });
    }


    public void DeactivateColl()
    {
        AudioManager.Instance.Pause("Gossip " + _gossipNumber);
        _boxCollider2D.enabled = false;
        sprRenderer.enabled = false;

        transform.DOKill();
    }

    private void Tremble()
    {
        transform.localScale = _originalScale;

        var targetSquish = new Vector3(transform.localScale.x + squishOffSet, transform.localScale.y,
            transform.localScale.z);
        transform.localScale = new Vector3(transform.localScale.x - squishOffSet, transform.localScale.y,
            transform.localScale.z);
        transform.DOScale(targetSquish, squishCd).SetLoops(-1, LoopType.Yoyo);


        var trans = transform.localEulerAngles;
        var targetTramble = new Vector3(trans.x, trans.y,
            trans.z + trambleOffSet);
        transform.localEulerAngles = new Vector3(trans.x, trans.y,
            trans.z - trambleOffSet);
        transform.DORotate(targetTramble, trambleCd).SetLoops(-1, LoopType.Yoyo);
    }

    private void CheckForWall()
    {
        /*var hit = Physics2D.CircleCast(end.position, 0.2f,-transform.up, rayDistance, groundMask);

        if (hit.collider)
        {
            PlayerAnt.hitWall = true;
        }*/
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