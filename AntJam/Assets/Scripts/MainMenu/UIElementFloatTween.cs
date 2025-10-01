using UnityEngine;
using DG.Tweening;

public class UIElementFloat : MonoBehaviour
{
    [Header("Configurações da Flutuação")]
    [Tooltip("A altura em pixels/unidades que o objeto vai subir a partir do ponto inicial.")]
    [SerializeField] private float floatHeight = 20f;

    [Tooltip("A duração de cada movimento (subida ou descida).")]
    [SerializeField] private float floatDuration = 2f;

    // 1. Variável para guardar a referência da nossa animação (tween)
    private Tween floatTween;

    void Start()
    {
        // 2. Guardamos a animação na nossa variável
        floatTween = transform.DOLocalMoveY(transform.localPosition.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // 3. Esta função é chamada automaticamente pela Unity quando o objeto é destruído
    private void OnDestroy()
    {
        // Matamos a animação para garantir que ela não continue rodando na memória
        if (floatTween != null)
        {
            floatTween.Kill();
        }
    }
}