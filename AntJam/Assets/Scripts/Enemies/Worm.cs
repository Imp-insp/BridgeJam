using System.Collections;
using UnityEngine;

public class Worm : MonoBehaviour
{
   [Header("Patrol Settings")] public Transform[] waypoints; // List of patrol points
    [SerializeField] private float speed = 2f; // Movement speed
    [SerializeField] private float restingTime = 2f;
    private bool resting;
    private Rigidbody2D rb;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Start at first waypoint
    }

    void FixedUpdate()
    {
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
        transform.position = waypoints[0].position;
        resting = true;
        yield return new WaitForSeconds(restingTime);
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
}
