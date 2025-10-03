using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Worm : MonoBehaviour
{
    [Header("Patrol Settings")] public Transform[] waypoints; // List of patrol points
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private float restingTime = 2f;
    private bool resting;
    private Rigidbody2D rb;

    [Header("Light")] [SerializeField] private Light2D lig2D;
    [SerializeField] private LayerMask groundMask;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Start at first waypoint
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.CircleCast(transform.position, 0.2f, transform.up, 0.3f, groundMask);
        lig2D.enabled = !hit.collider;
        if (!resting)
        {
            if (waypoints.Length < 2) return;


            Vector2 targetPos = waypoints[1].position;
            var newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            // Check if reached waypoint
            if (Vector2.Distance(rb.position, targetPos) < 0.1f)
            {
                StartCoroutine(ResetToFirstWaypoint());
            }
        }
    }

    private IEnumerator ResetToFirstWaypoint()
    {
        resting = true;
        yield return new WaitForSeconds(restingTime);
        transform.position = waypoints[0].position;
        resting = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        for (var i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer($"Ground"))
        {
            lig2D.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        lig2D.enabled = true;
    }
}