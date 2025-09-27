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

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // disable gravity since player sticks to walls
    }

    public void ProcessMovement(Vector2 direction)
    {
        // 1. Detect the closest surface beneath the player (relative to player’s local down)
        var hit = Physics2D.Raycast(transform.position, -transform.up, rayDistance, groundMask);
        Debug.DrawRay(transform.position, -transform.up, Color.red);
        
        if (hit.collider)
        {
            // 2. Stick to the surface
            var targetPos = hit.point + hit.normal * heightOffset; // offset by half height
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * stickForce);

            // 3. Align rotation with the surface normal
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stickForce);

            // 4. Move along the tangent
            var tangent = Vector2.Perpendicular(hit.normal);
            var input = direction.x; // A/D or arrow keys
            var velocity = tangent * -(input * moveSpeed);

            _rb.linearVelocity = velocity; // move along the surface
        }
        else
        {
            // If no surface detected, stop movement
            _rb.linearVelocity = Vector2.zero;
        }
    }
}