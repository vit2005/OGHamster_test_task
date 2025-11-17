using UnityEngine;
using DG.Tweening;

public class PopupUI : MonoBehaviour
{
    [Header("Animation")]
    public float Duration = 0.35f;
    public Ease EaseType = Ease.OutBack;
    public float StartScale = 0.7f;

    [SerializeField] private CanvasGroup group;
    private Tween showTween;

    public void Show()
    {
        showTween?.Kill(true);

        transform.localScale = Vector3.one * StartScale;
        group.alpha = 0f;
        gameObject.SetActive(true);

        showTween = DOTween.Sequence()
            .Join(group.DOFade(1f, Duration * 0.8f))                   // Fade-in
            .Join(transform.DOScale(1f, Duration).SetEase(EaseType));  // Zoom-out
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
