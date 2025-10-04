using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2.5f;

    private int _currentIndex;
    private Tween _currentTween;
    public List<Image> pannels;

    void Start()
    {
        // Garante que a imagem comece 100% opaca.
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1f);

        // Em vez de chamar o FadeIn() diretamente, n�s iniciamos uma Coroutine.
        StartCoroutine(StartFade());
    }

    // Coroutines s�o fun��es que podem ser pausadas.
    IEnumerator StartFade()
    {
        // A LINHA M�GICA: Pausa a fun��o e espera at� o pr�ximo frame ser renderizado.
        // Isso d� tempo para a cena carregar completamente antes de o fade come�ar.
        yield return null;

        // Agora que a cena est� est�vel, podemos chamar o fade.
        FadeIn();
    }

    public void FadeIn()
    {
        fadeImage.DOFade(0f, fadeDuration)
            .OnComplete(() => {
                fadeImage.gameObject.SetActive(false);
            });
    }

    public void Continue()
    {
        if (_currentTween.active)
        {
            _currentTween.Complete();
        }
        else if (_currentIndex ==  pannels.Count)
        {
            FadeImg(pannels[_currentIndex]);
        }
    }

    private void FadeImg(Image img)
    {
        _currentTween = img.DOFade(1f, fadeDuration).OnComplete(() =>
        {
            _currentIndex++;
        });
    }
}
