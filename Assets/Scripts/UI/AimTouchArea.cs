using UnityEngine;
using UnityEngine.EventSystems;

public class AimTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField]
    private RectTransform aimJoystickRoot;

    [SerializeField]
    private FloatingJoystick aimJoystick;

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransform canvasRect =
            aimJoystickRoot.parent as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        aimJoystick.MoveTo(localPoint);

        aimJoystick.OnPointerDown(eventData);

        GameInput.Instance.SetAimState(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        aimJoystick.OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        aimJoystick.OnPointerUp(eventData);

        GameInput.Instance.SetAimState(false);

        GameInput.Instance.SetAimDirection(Vector2.zero);
    }
}