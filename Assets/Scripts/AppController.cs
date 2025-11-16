using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    private static AppController _instance;
    public static AppController Instance => _instance;

    private UI _ui;
    private CameraRotator _cameraRotator;

    public void Awake()
    {
        Debug.Log("AppController Awake");
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(1);
    }

    public void RegisterUI(UI ui)
    {
        _ui = ui;
    }

    public void RegisterCameraRotator(CameraRotator cameraRotator)
    {
        _cameraRotator = cameraRotator;
    }

    public void HomeClick()
    {
        _cameraRotator.RotateTowardsMain();
    }

    public void MapClick()
    {
        _cameraRotator.RotateTowardsIcelands();
    }

    public void OpenMiningMinigame()
    {
        _ui.OpenMinigame();
    }

    public void OpenKYC()
    {
        _ui.OpenKnowYourClient();
    }
}