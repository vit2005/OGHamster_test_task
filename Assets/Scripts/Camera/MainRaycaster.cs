using UnityEngine;
using UnityEngine.EventSystems;

public class MainRaycaster : MonoBehaviour
{
    [SerializeField] private Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // --- Головний фільтр: клік по UI ---
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            // Для мобілки:
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
            }

            // --- Далі вже наш 3D клік ---
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
