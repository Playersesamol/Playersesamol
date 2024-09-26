using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool jumpPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        jumpPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        jumpPressed = false;
    }

    public bool IsJumpPressed()
    {
        return jumpPressed;
    }
}