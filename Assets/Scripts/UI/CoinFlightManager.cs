using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class CoinFlightManager : MonoBehaviour
{
    [Header("References")]
    public Transform StartPoint;
    public Transform EndPoint;
    public GameObject CoinPrefab;

    [Header("Settings")]
    public int PoolSize = 10;
    public float FlyDuration = 0.6f;
    public Ease FlyEase = Ease.OutQuad;

    private GameObject[] pool;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        pool = new GameObject[PoolSize];

        for (int i = 0; i < PoolSize; i++)
        {
            pool[i] = Instantiate(CoinPrefab, transform);
            pool[i].SetActive(false);
        }
    }

    private GameObject GetFreeCoin()
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if (!pool[i].activeSelf)
                return pool[i];
        }

        return null;
    }

    public void LaunchCoin()
    {
        SoundManager.Instance.Play(SoundType.Coin);

        GameObject coin = GetFreeCoin();
        if (coin == null)
        {
            AddScore();
            return;
        }

        coin.transform.position = StartPoint.position;
        coin.SetActive(true);

        // Летить
        coin.transform
            .DOMove(EndPoint.position, FlyDuration)
            .SetEase(FlyEase)
            .OnComplete(() =>
            {
                coin.SetActive(false);
                AddScore();
            });
    }

    private void AddScore()
    {
        AppController.Instance.Money.IncreaseMoney();
    }
}
