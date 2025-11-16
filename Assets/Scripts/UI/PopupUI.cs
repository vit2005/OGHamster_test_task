using UnityEngine;
using DG.Tweening;

public class PopupUI : MonoBehaviour
{
    [Header("Animation")]
    public float Duration = 0.35f;
    public Ease EaseType = Ease.OutBack; // OutBack = красиво і професійно
    public float StartScale = 0.7f;

    [SerializeField] private CanvasGroup group;
    private Tween showTween;

    public void Show()
    {
        // На всяк випадок — стопаємо все старе
        showTween?.Kill(true);

        // Підготовка
        transform.localScale = Vector3.one * StartScale;
        group.alpha = 0f;
        gameObject.SetActive(true);

        // Анімація
        showTween = DOTween.Sequence()
            .Join(group.DOFade(1f, Duration * 0.8f))                   // Fade-in
            .Join(transform.DOScale(1f, Duration).SetEase(EaseType))  // Zoom-out
            .SetUpdate(true); // Щоб працювало навіть на паузі, якщо треба
    }

    public void Hide()
    {
        showTween?.Kill(true);

        showTween = DOTween.Sequence()
            .Join(group.DOFade(0f, Duration * 0.6f))
            .Join(transform.DOScale(StartScale, Duration * 0.6f))
            .OnComplete(() => gameObject.SetActive(false));
    }
}
