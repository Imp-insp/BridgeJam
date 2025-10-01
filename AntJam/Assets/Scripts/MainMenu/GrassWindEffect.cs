using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class GrassWindEffect : MonoBehaviour
{
    [Header("Configurações do Vento")]
    [SerializeField] private float swayAngle = 5f;
    [SerializeField] private float swayDuration = 3f;
    [SerializeField] private float maxStartDelay = 1f;

    private Transform pivotParent;
    private Tween swayTween; // 1. Variável para guardar a referência da animação

    private void Awake()
    {
        AdjustPivot();
    }

    private void Start()
    {
        StartSwayAnimation();
    }

    // 3. Método para limpar a animação da memória quando o objeto é destruído
    private void OnDestroy()
    {
        // O ?.Kill() é um atalho seguro para: if (swayTween != null) swayTween.Kill();
        swayTween?.Kill();
    }

    private void AdjustPivot()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("O objeto não tem um sprite para ajustar o pivô.", this);
            return;
        }

        GameObject pivotGO = new GameObject(name + "_Pivot");
        pivotGO.transform.position = transform.position;
        pivotParent = pivotGO.transform;
        transform.SetParent(pivotParent);
        transform.localPosition = Vector3.zero;
    }

    private void StartSwayAnimation()
    {
        if (pivotParent == null) return;

        // 2. Guardamos a animação na nossa variável
        swayTween = pivotParent.DOLocalRotate(
            new Vector3(0, 0, swayAngle),
            swayDuration / 2f
        )
        .SetEase(Ease.InOutSine)
        .SetLoops(-1, LoopType.Yoyo)
        .SetDelay(Random.Range(0, maxStartDelay));
    }
}