using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject bottomPanel;
    [SerializeField] private GameObject InDevelopmentPopUp;
    [SerializeField] private GameObject miningMinigame;

    private void Awake()
    {
        AppController.Instance.RegisterUI(this);
    }

    public void OnMapClick()
    {
        AppController.Instance.MapClick();
    }

    public void OnHomeClick()
    {
        AppController.Instance.HomeClick();
    }

    public void OnVolumeClick()
    {
        ShowInDevelopmentPopUp();
    }

    public void OnCloseMinigameClick()
    {
        miningMinigame.SetActive(false);
    }

    public void OpenKnowYourClient()
    {
        ShowInDevelopmentPopUp();
    }

    public void OpenMinigame()
    {
        miningMinigame.SetActive(true);
    }

    public void ShowInDevelopmentPopUp()
    {
        InDevelopmentPopUp.SetActive(true);
    }

}
