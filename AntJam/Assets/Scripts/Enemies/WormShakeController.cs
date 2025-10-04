using Unity.Cinemachine;
using UnityEngine;

public class WormShakeController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    
    [Header("Configura��o do Tremor")]
    [Tooltip("A intensidade m�xima do tremor quando o jogador est� no centro.")]
    [SerializeField] private float maxShakeIntensity = 0.3f;

    [Tooltip("Frequ�ncia do tremor (tremores por segundo).")]
    [SerializeField] private float shakeFrequency = 12f;

    [Header("Refer�ncias")]
    [Tooltip("A tag do objeto do jogador.")]
    [SerializeField] private string playerTag = "Player";

    private CinemachineImpulseSource impulseSource;
    private Transform playerTransform;
    private CapsuleCollider2D shakeCollider;
    private bool isPlayerInside = false;
    private float shakeTimer = 0f;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        shakeCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        transform.position = target.transform.position;
        
        if (isPlayerInside && playerTransform)
        {
            shakeTimer += Time.deltaTime;

            // Gera impulsos r�pidos baseado na frequ�ncia
            float shakeInterval = 1f / shakeFrequency;
            if (shakeTimer >= shakeInterval)
            {
                shakeTimer = 0f;

                var distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                var maxDistance = Mathf.Max(shakeCollider.size.x, shakeCollider.size.y) / 2f;
                var intensityFactor = Mathf.InverseLerp(maxDistance, 0, distanceToPlayer);
                var currentIntensity = intensityFactor * maxShakeIntensity + 0.2f;
                var currentIntensityMuysuc = intensityFactor * maxShakeIntensity;
                AudioManager.Instance.PlayOnce("Tremor", Mathf.Abs( currentIntensityMuysuc));
                if (currentIntensity > 0)
                {
                    impulseSource.GenerateImpulse(currentIntensity);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = other.transform;
            isPlayerInside = true;
            shakeTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = false;
            playerTransform = null;
            shakeTimer = 0f;
            AudioManager.Instance.Pause("Tremor");
        }
    }
}