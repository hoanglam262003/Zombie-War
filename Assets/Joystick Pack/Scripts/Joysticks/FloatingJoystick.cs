using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 defaultBackgroundPos;
    protected override void Start()
    {
        base.Start();
        defaultBackgroundPos = background.anchoredPosition;
    }

    public void MoveTo(Vector2 localPoint)
    {
        background.anchoredPosition = localPoint;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        background.anchoredPosition = defaultBackgroundPos;
    }
}