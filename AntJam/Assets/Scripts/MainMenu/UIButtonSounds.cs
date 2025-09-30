using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip hoverSound;

    // A MUDANÇA PRINCIPAL ESTÁ AQUI:
    // Trocamos um único AudioClip por um array (lista) de AudioClips.
    [SerializeField] private AudioClip[] clickSounds;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxAudioSource;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (sfxAudioSource == null)
        {
            sfxAudioSource = Object.FindFirstObjectByType<AudioSource>();
        }
    }

    void OnEnable()
    {
        button.onClick.AddListener(PlayClickSounds); // Mudamos o nome da função para o plural
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(PlayClickSounds);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(hoverSound);
        }
    }

    // Esta função agora percorre a lista e toca todos os sons.
    private void PlayClickSounds()
    {
        if (clickSounds != null && sfxAudioSource != null)
        {
            // Loop "foreach" que passa por cada clipe de áudio na nossa lista.
            foreach (AudioClip clip in clickSounds)
            {
                if (clip != null)
                {
                    sfxAudioSource.PlayOneShot(clip);
                }
            }
        }
    }
}