using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Button_Float : UI_Button
{
    [SerializeField, NonEditable] private Vector2 initialOffsetMax;
    [SerializeField, NonEditable] private Vector2 initialOffsetMin;
    [SerializeField] private RectTransform targetRect;

    protected override void Awake()
    {
        base.Awake();

        imageButton.trigger.onDown += OnPressed;
        imageButton.trigger.onUp += OnPressingToRelease;
        imageButton.trigger.onExit += OnExit;


        initialOffsetMax = buttonImage.rectTransform.offsetMax;
        initialOffsetMin = buttonImage.rectTransform.offsetMin;
        TransformToInitial();
    }

    public override void Event_Pressable()
    {
        TransformToInitial();
    }
    public override void Event_UnPressable()
    {
        TransformToTarget();
    }

    private void TransformToInitial()
    {
        buttonImage.rectTransform.offsetMax = initialOffsetMax;
        buttonImage.rectTransform.offsetMin = initialOffsetMin;
    }

    private void TransformToTarget()
    {
        buttonImage.rectTransform.offsetMax = targetRect.offsetMax;
        buttonImage.rectTransform.offsetMin = targetRect.offsetMin;
    }

    private void OnPressed()
    {
        if (imageButton.Pressable == false)
        {
            return;
        }


        TransformToTarget();
        isPress = true;
    }

    private void OnExit()
    {
        if (imageButton.Pressable == false)
        {
            return;
        }

        TransformToInitial();
        isPress = false;
    }

    private void OnPressingToRelease()
    {
        if (imageButton.Pressable == false)
        {
            return;
        }

        TransformToInitial();

        if (isPress == true)
        {
            imageButton.onClickAction?.Invoke();
        }
    }

}
