using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;    

public class SceneFadeController : MonoBehaviour
{

    public Image fadeImage;

    // A duração do nosso fade em segundos.
    public float fadeDuration = 2.5f;

    void Start()
    {
        // Garante que a imagem esteja totalmente opaca antes de começar o fade.
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        // Inicia o fade-in suave
        FadeIn();
    }

    public void FadeIn()
    {

        fadeImage.DOFade(0f, fadeDuration)
            .OnComplete(() => {
                // Ao final da animação, desativamos o objeto da imagem.
                fadeImage.gameObject.SetActive(false);
            });
    }
}