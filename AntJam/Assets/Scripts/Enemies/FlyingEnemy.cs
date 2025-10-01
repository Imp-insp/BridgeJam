using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    [Header("Patrol Settings")] public Transform[] waypoints; // List of patrol points
    public float speed = 2f; // Movement speed
    public float waitTime = 1f; // Time to wait at each point
    public bool loop = true; // If false, will ping-pong

    [Header("Optional")] 
    public bool stopsAtPatrol = false;
    public bool flipOnDirectionChange = true; // Flip sprite when turning

    private int currentIndex = 0;
    private bool goingForward = true;
    private float waitCounter = 0f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position; // Start at first waypoint
    }

    void FixedUpdate()
    {
        if (waypoints.Length < 2) return;

        if (waitCounter > 0f && stopsAtPatrol)
        {
            waitCounter -= Time.fixedDeltaTime;
            return;
        }

        Vector2 targetPos = waypoints[currentIndex].position;
        var newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Check if reached waypoint
        if (Vector2.Distance(rb.position, targetPos) < 0.1f)
        {
            waitCounter = waitTime;
            GetNextWaypoint();
        }

        
        // Flip sprite if needed
        var side = transform.position.x - newPos.x;
        if (flipOnDirectionChange && side != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(side) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void GetNextWaypoint()
    {
        if (loop)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
        else
        {
            if (goingForward)
            {
                if (currentIndex < waypoints.Length - 1) currentIndex++;
                else
                {
                    currentIndex--;
                    goingForward = false;
                }
            }
            else
            {
                if (currentIndex > 0) currentIndex--;
                else
                {
                    currentIndex++;
                    goingForward = true;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw lines between waypoints in editor
        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}