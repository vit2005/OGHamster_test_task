using UnityEngine;
using DG.Tweening;

public class CameraRotator : MonoBehaviour
{
    [Header("Target rotations")]
    public Vector3 MainRotation;
    public Vector3 IcelandsRotation;

    [Header("Animation settings")]
    public float RotateDuration = 1f;
    public Ease RotateEase = Ease.InOutSine;

    private Tween _currentTween;
    private Transform _cam;

    private void Awake()
    {
        _cam = Camera.main.transform;
        AppController.Instance.RegisterCameraRotator(this);
    }

    public void RotateTowardsMain()
    {
        RotateTo(MainRotation);
    }

    public void RotateTowardsIcelands()
    {
        RotateTo(IcelandsRotation);
    }

    private void RotateTo(Vector3 targetRotation)
    {
        // якщо користувач передумав Ч прибиваЇмо попередн≥й тв≥н
        _currentTween?.Kill();

        // —тартуЇмо новий
        _currentTween = _cam
            .DORotate(targetRotation, RotateDuration)
            .SetEase(RotateEase);
    }
}
