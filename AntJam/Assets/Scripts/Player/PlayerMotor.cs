using System;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float heightOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stickForce = 10f; // How strongly we stick to surfaces
    [SerializeField] private float rayDistance; // Distance to check for a surface

    [Header("Ref")] [SerializeField] private LayerMask groundMask; // Layers considered as walkable
    [SerializeField] private Transform sidables;
    private Rigidbody2D _rb;
    [SerializeField] private Animator sidaAnim;

    [Header("Animation")] private bool _isWalking;
    
    
    [Header("Control")] public static bool WalkingOnAnts;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    public int flip;
    public int passedFlip;
    private Vector2 _tangent;
    private float _originalRayDistance;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // disable gravity since player sticks to walls
        
        _originalRayDistance = rayDistance;
    }

    private void Update()
    {
        sidaAnim.SetBool(IsWalking, _isWalking);
    }

    public void ProcessMovement(Vector2 direction)
    {
        if (_tangent.x > 0)
        {
            flip = -1;
        }
        else if (_tangent.x < 0)
        {
            flip = 1;
        }
        
        var hit = Physics2D.CircleCast(transform.position, 0.2f,-transform.up, rayDistance, groundMask);
       
        if (hit.collider)
        {
            rayDistance = _originalRayDistance;
            WalkingOnAnts =  hit.collider.gameObject.layer == 6;
            // Stick to the surface
            var targetPos = hit.point + hit.normal * heightOffset; 
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * stickForce);

            // Align rotation with the surface normal
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stickForce);

            _tangent = Vector2.Perpendicular(hit.normal);
            var input = -direction.x * passedFlip;
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
            // If no surface detected, stop movement
            rayDistance++;
        }
    }

    public void StartMovement()
    {
        passedFlip = flip;
    }
}