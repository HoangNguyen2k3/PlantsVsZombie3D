using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private StarterAssetsInputs starterInputs;
    [SerializeField] private float swipeSensitivity = 0.1f;
    [SerializeField] private bool onlyRightSide = true;

    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData) {
        if (onlyRightSide && eventData.position.x < Screen.width / 2f)
            return;

        isDragging = true;
        if (starterInputs != null)
            starterInputs.UseTouchLook(true);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!isDragging || starterInputs == null)
            return;

        Vector2 lookDelta = -eventData.delta * swipeSensitivity;
        starterInputs.LookInput(lookDelta);
    }

    public void OnPointerUp(PointerEventData eventData) {
        isDragging = false;

        if (starterInputs != null) {
            starterInputs.LookInput(Vector2.zero);
            starterInputs.UseTouchLook(false);
        }
    }
}
