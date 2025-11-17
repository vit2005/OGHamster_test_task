using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private PopupUI InDevelopmentPopUp;
    [SerializeField] private GameObject miningMinigame;

    private void Awake()
    {
        AppController.Instance.RegisterUI(this);
    }

    // Setted from editor
    public void OnMapClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        AppController.Instance.MapClick();
    }

    // Setted from editor
    public void OnHomeClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        AppController.Instance.HomeClick();
    }

    // Setted from editor
    public void OnVolumeClick()
    {
        ShowInDevelopmentPopUp();
    }

    // Called from AppController
    public void OpenKnowYourClient()
    {
        ShowInDevelopmentPopUp();
    }

    // Called from AppController
    public void OpenMinigame()
    {
        SoundManager.Instance.Play(SoundType.Play);
        miningMinigame.SetActive(true);
        bottomPanel.SetActive(false);
    }

    // Setted from editor
    public void OnCloseMinigameClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        bottomPanel.SetActive(true);
        miningMinigame.SetActive(false);
    }

    private void ShowInDevelopmentPopUp()
    {
        SoundManager.Instance.Play(SoundType.Error);
        InDevelopmentPopUp.Show();
    }

}
