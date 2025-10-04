using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotor : MonoBehaviour
{
    
    public static PlayerMotor Instance;
    [Header("Movement")] 
    [SerializeField] private float heightOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stickForce = 10f; // How strongly we stick to surfaces
    [SerializeField] private float rayDistance; // Distance to check for a surface

    public static bool invertedMovement;
    
    [Header("Ref")] [SerializeField] private LayerMask groundMask; // Layers considered as walkable
    [SerializeField] private LayerMask realGroundMask;
    [SerializeField] private Transform sidables;
    private Rigidbody2D _rb;
    [SerializeField] private Animator sidaAnim;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TextMeshProUGUI secretsFoundText;

    [Header("Animation")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private float downsScalingAmount;
    [SerializeField] private float animationTime;
    private bool _isWalking;

    [Header("EndScene")]
    [SerializeField] private GameObject endScene;
    [SerializeField] private GameObject endSceneSecret;
    [SerializeField] private GameObject redSecret;
    [SerializeField] private Image[] flowerUis;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Control")] public static bool WalkingOnAnts;
    public static float DistanceToGround;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    public int flip;
    public int passedFlip;
    private Vector2 _tangent;
    private float _originalRayDistance;
    private bool startedEnding;
    private Vector3 originalScale;

    private float playerTimer;

    private void Awake()
    {
        Instance = this;
        originalScale = transform.localScale;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // disable gravity since player sticks to walls
        
        _originalRayDistance = rayDistance;
    }

    private void Update()
    {
        playerTimer += Time.deltaTime;
        sidaAnim.SetBool(IsWalking, _isWalking);

        if (_isWalking && !startedEnding)
        {
            AudioManager.Instance.PlayOnce("Walk");
        }
        else
        {
            AudioManager.Instance.Pause("Walk");
        }
        
    }

    public void ProcessMovement(Vector2 direction)
    {
        if (startedEnding) return;
        var hit = Physics2D.CircleCast(transform.position, 0.2f, -transform.up, rayDistance, groundMask);

        if (hit.collider)
        {
            DistanceToGround = hit.distance;
            rayDistance = _originalRayDistance;
            WalkingOnAnts = hit.collider.gameObject.layer == 6;


            if (_tangent.x > 0)
            {
                flip = -1;
            }
            else if (_tangent.x < 0)
            {
                flip = 1;
            }



            // Stick to the surface
            var targetPos = hit.point + hit.normal * heightOffset;
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * stickForce);

            // Align rotation with the surface normal
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stickForce);

            _tangent = Vector2.Perpendicular(hit.normal);
            var input = invertedMovement ? -direction.x * passedFlip : -direction.x;
            var velocity = _tangent * (input * moveSpeed);
            if (direction.x != 0)
            {
                _isWalking = true;
                sidables.localRotation = -input < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _isWalking = false;
            }

            _rb.linearVelocity = velocity; // move along the surface
        }
        else
        {
            DistanceToGround = float.MaxValue;
            // --- RECOVERY MODE: Initial cast failed, meaning the player is falling or floating off a gap. ---

            // 1. Perform a wide-range search to find the absolute NEAREST valid surface.
            // Use MAX_RECOVERY_DISTANCE (e.g., 5f) to prevent searching the entire scene.
            var allHits = Physics2D.CircleCastAll(
                transform.position,
                1f,
                -transform.up,
                0.4f,
                groundMask
            );

            if (allHits.Length > 0)
            {
                // 2. Find the closest hit among all results.
                RaycastHit2D nearestHit = allHits[0];
                float minDist = nearestHit.distance;

                for (int i = 1; i < allHits.Length; i++)
                {
                    if (allHits[i].distance < minDist)
                    {
                        minDist = allHits[i].distance;
                        nearestHit = allHits[i];
                    }
                }

                // 3. Snap the player to the nearest surface found.

                // Teleport the player immediately to the calculated position of the nearest surface.
                // This prevents the jarring movement over many frames.
                var snapPos = nearestHit.point + nearestHit.normal * heightOffset;
                transform.position = snapPos;

                // Snap rotation immediately
                var snapRotation = Quaternion.FromToRotation(transform.up, nearestHit.normal) * transform.rotation;
                transform.rotation = snapRotation;

                // Reset state
                rayDistance = _originalRayDistance;
                _rb.linearVelocity = Vector2.zero; // Stop movement until new input is received

            }
            else
            {
                // No surfaces found even in the max recovery range.
                // Reset distance and allow player to fall normally.
                rayDistance = _originalRayDistance;
                // Optionally, ensure gravity is turned on here if it was off during wall-hugging.
                // _rb.gravityScale = 1f; 
            }
        }
    }

    public void EndAnimation(Vector2 trgtPos)
    {
        if (startedEnding) return;
        
        _rb.linearVelocity = Vector2.zero;
        startedEnding = true;

        transform.position = trgtPos;
        
        var targetScale = new Vector3(transform.localScale.x - downsScalingAmount, transform.localScale.y - downsScalingAmount);
        var targetColor = new Color(0, 0, 0, 0);

        transform.DORotate(new Vector3(0,0,transform.rotation.eulerAngles.z + rotationAmount), animationTime, RotateMode.FastBeyond360);
        transform.DOScale(targetScale, animationTime);
        _renderer.DOColor(targetColor, animationTime).OnComplete(OpenEndScene);
        
    }

    private void OpenEndScene()
    {
        endScene.SetActive(true);
        var sFound = PlayerManager.Instance.secretsFound;
        secretsFoundText.text = "Secrets Found: " + sFound + "/" + 4;
        for (var i = 0; i < sFound ; i++)
        {
            flowerUis[i].color = Color.white;
            if (sFound == 5)
            {
                flowerUis[i].color = Color.red;
            }
        }
        if (sFound == 4)
        {
            endSceneSecret.SetActive(true);
        }

        if (sFound == 5)
        {
            redSecret.SetActive(true);
        }
        
        
        timerText.text = "Finished in " + (int) playerTimer/60 + " minutes!"; 
        
       
    }

    public void Originate()
    {
        transform.DOScale(originalScale, 0f);
        _renderer.color = Color.white;
        startedEnding = false;
        
        endScene.SetActive(false);
        
        PlayerManager.Instance.Die();
        
        
    }
    public void StartMovement()
    {
        passedFlip = flip;
    }
    public static float GetCurrentDistanceToGround()
    {
        // A �nica mudan�a � aqui: trocamos "Instance.groundMask" por "Instance.realGroundMask"
        var hit = Physics2D.CircleCast(
            Instance.transform.position,
            0.2f,
            -Instance.transform.up, // <-- Mude de volta de Vector2.down para isto
            100f,
            Instance.realGroundMask
        );

        if (hit.collider)
        {
            return hit.distance;
        }

        return float.MaxValue;
    }
}