using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 90f;
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
