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

    public void OnMapClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        AppController.Instance.MapClick();
    }

    public void OnHomeClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        AppController.Instance.HomeClick();
    }

    public void OnVolumeClick()
    {
        ShowInDevelopmentPopUp();
    }

    public void OnCloseMinigameClick()
    {
        SoundManager.Instance.Play(SoundType.Click);
        miningMinigame.SetActive(false);
    }

    public void OpenKnowYourClient()
    {
        ShowInDevelopmentPopUp();
    }

    public void OpenMinigame()
    {
        SoundManager.Instance.Play(SoundType.Play);
        miningMinigame.SetActive(true);
    }

    public void ShowInDevelopmentPopUp()
    {
        SoundManager.Instance.Play(SoundType.Error);
        InDevelopmentPopUp.Show();
    }

}
