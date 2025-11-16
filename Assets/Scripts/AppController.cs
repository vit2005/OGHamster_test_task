using UnityEngine;

public class AppController : MonoBehaviour
{
    public void Awake()
    {
        Debug.Log("AppController Awake");
        DontDestroyOnLoad(this.gameObject);
    }
}