using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Essencial para detectar o evento de hover

// Garante que este script só pode ser adicionado a um GameObject que tenha um componente Button
[RequireComponent(typeof(Button))]
public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler // Implementa a interface para o evento de "mouse enter"
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Audio Source")]
    // Opcional: Se você quiser que todos os botões usem o mesmo AudioSource
    [SerializeField] private AudioSource sfxAudioSource;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        // Se nenhum AudioSource específico foi arrastado, tenta encontrar um na cena
        // Isso evita a necessidade de ter um AudioSource em cada botão
        if (sfxAudioSource == null)
        {
            sfxAudioSource = FindFirstObjectByType<AudioSource>();
        }
    }

    void OnEnable()
    {
        // Adiciona a função de tocar o som de clique ao evento OnClick do botão
        button.onClick.AddListener(PlayClickSound);
    }
    
    void OnDisable()
    {
        // Remove a função para evitar erros quando o objeto for desativado
        button.onClick.RemoveListener(PlayClickSound);
    }

    // Esta função é chamada automaticamente por causa da interface IPointerEnterHandler
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && sfxAudioSource != null)
        {
            // PlayOneShot é ideal para efeitos sonoros, pois não interrompe outras músicas
            sfxAudioSource.PlayOneShot(hoverSound);
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clickSound);
        }
    }
}