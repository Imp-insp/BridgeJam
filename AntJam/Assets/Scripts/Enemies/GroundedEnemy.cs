using System;
using UnityEngine;

public class GroundedEnemy : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float heightOffset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float stickForce = 10f; // How strongly we stick to surfaces
    [SerializeField] private float rayDistance; // Distance to check for a surface

    [Header("Ref")] [SerializeField] private LayerMask groundMask; // Layers considered as walkable
    [SerializeField] private Transform sidebles; 
    private Rigidbody2D _rb;

    [Header("Control")] [SerializeField] private bool rightDirection;
    [SerializeField] private bool isPatrolling;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0; // disable gravity since player sticks to walls
    }

    private void Update()
    {
        if (isPatrolling) ProcessMovement();
    }

    private void ProcessMovement()
    {
        var hit = Physics2D.CircleCast(transform.position, 0.2f, -transform.up, rayDistance, groundMask);

        if (hit.collider)
        {
            // Stick to the surface
            var targetPos = hit.point + hit.normal * heightOffset; // offset by half height
            transform.position = Vector2.Lerp(transform.position, targetPos, Time.fixedDeltaTime * stickForce);

            // Align rotation with the surface normal
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * stickForce);

            var tangent = Vector2.Perpendicular(hit.normal);
            var input = rightDirection ? 1 : -1;
            sidebles.localRotation = input < 0 ? Quaternion.Euler(0,  180,0 ) : Quaternion.Euler(0, 0, 0);;
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