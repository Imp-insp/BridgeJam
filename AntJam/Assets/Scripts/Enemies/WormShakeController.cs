using Unity.Cinemachine;
using UnityEngine;

public class WormShakeController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Configuração do Tremor")]
    [Tooltip("A intensidade máxima do tremor quando o jogador está no centro.")]
    [SerializeField] private float maxShakeIntensity = 0.3f;

    [Tooltip("O volume máximo do som de tremor (0 a 1).")]
    [SerializeField] private float maxSoundVolume = 1f;

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
        if (target != null)
        {
            transform.position = target.position;
        }

        // Se o jogador estiver dentro, atualiza o shake e o som
        if (isPlayerInside && playerTransform != null)
        {
            UpdateShakeAndSound();
        }
    }

    private void UpdateShakeAndSound()
    {
        shakeTimer += Time.deltaTime;
        float shakeInterval = 1f / shakeFrequency;

        if (shakeTimer >= shakeInterval)
        {
            shakeTimer = 0f;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            float maxDistance = Mathf.Max(shakeCollider.size.x, shakeCollider.size.y) / 2f;
            float intensityFactor = Mathf.Clamp01(Mathf.InverseLerp(maxDistance, 0, distanceToPlayer));

            // Aplica o shake na câmera
            float currentIntensity = intensityFactor * maxShakeIntensity;
            if (currentIntensity > 0 && impulseSource != null)
            {
                impulseSource.GenerateImpulse(currentIntensity);
            }

            // Atualiza o volume do som
            float currentVolume = intensityFactor * maxSoundVolume;
            AudioManager.Instance.SetVolume("Tremor", currentVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerTransform = other.transform;
            isPlayerInside = true;

            // [CORREÇÃO DO CLIP] Inicia o som e imediatamente zera o volume.
            // O Update() vai ajustar para o volume correto no frame seguinte, sem o pico de 100%.
            AudioManager.Instance.Play("Tremor");
            AudioManager.Instance.SetVolume("Tremor", 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInside = false;
            playerTransform = null;

            // Simplesmente para o som ao sair, sem fade.
            AudioManager.Instance.Stop("Tremor");
        }
    }

    private void OnDestroy()
    {
        // Garante que o som pare se o objeto for destruído
        if (AudioManager.Instance != null && AudioManager.Instance.IsPlaying("Tremor"))
        {
            AudioManager.Instance.Stop("Tremor");
        }
    }
}