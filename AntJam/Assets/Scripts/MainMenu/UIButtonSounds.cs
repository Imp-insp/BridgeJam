using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
public class UIButtonSounds : MonoBehaviour, IPointerEnterHandler
{
    private static List<UIButtonSounds> allInstances = new List<UIButtonSounds>();

    public static void DisableAllButtons()
    {
        foreach (UIButtonSounds instance in allInstances)
        {
            if (instance != null)
            {
                instance.button.interactable = false;
            }
        }
    }

    public static void EnableAllButtons()
    {
        foreach (UIButtonSounds instance in allInstances)
        {
            if (instance != null)
            {
                instance.button.interactable = true;
            }
        }
    }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip[] clickSounds;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxAudioSource;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        if (sfxAudioSource == null)
        {
            sfxAudioSource = FindFirstObjectByType<AudioSource>();
        }
    }

    void OnEnable()
    {
        if (!allInstances.Contains(this))
        {
            allInstances.Add(this);
        }
        button.onClick.AddListener(PlayClickSounds);
    }

    void OnDisable()
    {
        allInstances.Remove(this);
        button.onClick.RemoveListener(PlayClickSounds);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // GARANTIA DUPLA:
        // Se o botão não estiver interativo, a função para aqui e não toca o som.
        if (!button.interactable)
        {
            return;
        }

        if (hoverSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(hoverSound);
        }
    }

    private void PlayClickSounds()
    {
        if (clickSounds != null && sfxAudioSource != null)
        {
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