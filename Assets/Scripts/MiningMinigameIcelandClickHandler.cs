using UnityEngine;

public class MiningMinigameIcelandClickHandler : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        AppController.Instance.OpenMiningMinigame();
    }
}
