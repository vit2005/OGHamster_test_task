using UnityEngine;
using UnityEngine.EventSystems;

public class MainRaycaster : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the click is over UI, and ignore it if so
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // For touch input, also check if over UI
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
            }

            // Now perform the raycast
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                    clickable.OnClick();
            }
        }
    }
}
