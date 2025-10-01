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

    [Header("Control")] public static bool walkingOnAnts;
    public int flipped;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // disable gravity since player sticks to walls
    }

    public void ProcessMovement(Vector2 direction)
    {
        var hit = Physics2D.CircleCast(transform.position, 0.2f,-transform.up, rayDistance, groundMask);
        
        if (hit.collider)
        {
            walkingOnAnts =  hit.collider.gameObject.layer == 6;
            // Stick to the surface
            var targetPos = hit.point + hit.normal * heightOffset; // offset by half height
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * stickForce);

            // Align rotation with the surface normal
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stickForce);

            var tangent = Vector2.Perpendicular(hit.normal);
            var input = -direction.x * flipped;
            var velocity = tangent * (input * moveSpeed);
            if (direction.x != 0) sidables.localRotation = -input < 0 ? Quaternion.Euler(0,  180,0 ) : Quaternion.Euler(0, 0, 0);
            _rb.linearVelocity = velocity; // move along the surface
        }
        else
        {
            // If no surface detected, stop movement
            _rb.linearVelocity = Vector2.zero;
        }
    }

    public void StartMovement()
    {
       
        if (Math.Abs(transform.eulerAngles.z) > 92)
        {
            flipped = -1;
        }
        else
        {
            flipped = 1;
        }
    }
}