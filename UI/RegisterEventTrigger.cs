using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RegisterEventTrigger : EventTrigger
{

    public Action onEnter;
    public Action onExit;
    public Action onClick;
    public Action onUp;
    public Action onDown;


    private void Start()
    {

    }
    public override void OnPointerEnter(PointerEventData _data)
    {
        base.OnPointerEnter(_data);
        onEnter?.Invoke();
    }

    public override void OnPointerExit(PointerEventData _data)
    {
        base.OnPointerExit(_data);
        onExit?.Invoke();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        onClick?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onUp?.Invoke();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onDown?.Invoke();
    }

    [ContextMenu(nameof(AddContent))]
    public void AddContent()
    {

    }
}
