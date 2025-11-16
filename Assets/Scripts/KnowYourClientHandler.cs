using UnityEngine;

public class KnowYourClientHandler : MonoBehaviour, IClickable
{
    public void OnClick()
    {
        AppController.Instance.OpenKYC();
    }
}
