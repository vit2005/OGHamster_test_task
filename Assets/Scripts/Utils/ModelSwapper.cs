using System.Collections.Generic;
using UnityEngine;

public class ModelSwapper : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToSwap;
    [SerializeField] GameObject currentActiveObject;
    [SerializeField] int currentIndex = 0;
    [SerializeField] float durationBetweenSwaps;
    float _lastTimeSwap = 1f;

    void Update()
    {
        if (_lastTimeSwap > durationBetweenSwaps)
        {
            _lastTimeSwap = 0f;
            currentActiveObject.SetActive(false);
            currentIndex = Random.Range(0, objectsToSwap.Count);
            currentActiveObject = objectsToSwap[currentIndex];
            currentActiveObject.SetActive(true);
        }
        else
        {
            _lastTimeSwap += Time.deltaTime;
        }
    }
}
