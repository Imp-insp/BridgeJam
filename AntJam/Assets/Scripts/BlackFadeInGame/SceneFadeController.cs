using System.Collections; // Necessário para usar Coroutines (IEnumerator)
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneFadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.5f;

    void Start()
    {
        // Garante que a imagem comece 100% opaca.
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        // Em vez de chamar o FadeIn() diretamente, nós iniciamos uma Coroutine.
        StartCoroutine(StartFade());
    }

    // Coroutines são funções que podem ser pausadas.
    IEnumerator StartFade()
    {
        // A LINHA MÁGICA: Pausa a função e espera até o próximo frame ser renderizado.
        // Isso dá tempo para a cena carregar completamente antes de o fade começar.
        yield return null;

        // Agora que a cena está estável, podemos chamar o fade.
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.DOFade(0f, fadeDuration)
            .OnComplete(() => {
                fadeImage.gameObject.SetActive(false);
            });
    }
}