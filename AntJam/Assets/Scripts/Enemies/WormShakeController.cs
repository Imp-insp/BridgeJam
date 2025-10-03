using Unity.Cinemachine;
using UnityEngine;

public class WormShakeController : MonoBehaviour
{
    [Header("Configuração do Tremor")]
    [Tooltip("A intensidade máxima do tremor quando o jogador está no centro.")]
    [SerializeField] private float maxShakeIntensity = 0.3f;

    [Tooltip("Frequência do tremor (tremores por segundo).")]
    [SerializeField] private float shakeFrequency = 12f;

    [Header("Referências")]
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
        if (isPlayerInside && playerTransform != null)
        {
            shakeTimer += Time.deltaTime;

            // Gera impulsos rápidos baseado na frequência
            float shakeInterval = 1f / shakeFrequency;
            if (shakeTimer >= shakeInterval)
            {
                shakeTimer = 0f;

                float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
                float maxDistance = Mathf.Max(shakeCollider.size.x, shakeCollider.size.y) / 2f;
                float intensityFactor = Mathf.InverseLerp(maxDistance, 0, distanceToPlayer);
                float currentIntensity = intensityFactor * maxShakeIntensity;

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
        }
    }
}