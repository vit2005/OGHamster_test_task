using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class MoneyVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    public float ScorePopScale = 1.2f;
    public float ScorePopDuration = 0.1f;

    public void Awake()
    {
        MoneyProvider.OnMoneyChanged += UpdateMoneyDisplay;
    }

    private void UpdateMoneyDisplay(int obj)
    {
        moneyText.SetText(obj.ToString());

        // Visual effect
        moneyText.transform.DOKill(); 
        moneyText.transform.localScale = Vector3.one;

        moneyText.transform
            .DOScale(ScorePopScale, ScorePopDuration)
            .SetLoops(2, LoopType.Yoyo);
    }
}
