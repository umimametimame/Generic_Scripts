using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(RegisterEventTrigger))]
public class ImageButton : MonoBehaviour
{
    private Image buttonImage;
    public UI_ImageOperator images = new UI_ImageOperator();
    [SerializeField, NonEditable] private Vector2 initialOffsetMax;
    [SerializeField, NonEditable] private Vector2 initialOffsetMin;
    [SerializeField] private RectTransform targetRect;
    private RegisterEventTrigger trigger;

    /// <summary>
    /// É{É^ÉìÇ™âüÇπÇ»Ç¢èÍçáÇ™ë∂ç›Ç∑ÇÈÇ©
    /// </summary>
    public bool isPressableConditions;
    private bool pressable;
    public bool Pressable
    {
        get
        {
            if(isPressableConditions == true)
            {
                return pressable;
            }

            return true;
        }
    }
    public ValueChecker<bool> pressableCheck;
    private bool isPress;
    public Action clickUpAction;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        images.Assign(gameObject);
        trigger = GetComponent<RegisterEventTrigger>();

        trigger.onDown += Pressed;
        trigger.onUp += UnPressed;
        trigger.onExit += Exit;

        initialOffsetMax = buttonImage.rectTransform.offsetMax;
        initialOffsetMin = buttonImage.rectTransform.offsetMin;

        isPress = false;

        pressableCheck.Initialize(Pressable);
        pressableCheck.changedAction += Event_PressableJudge;
        Event_PressableJudge();
    }

    public void Update()
    {
        if(isPressableConditions == true)
        {
            pressableCheck.Update(Pressable);

        }
    }

    public void AssignPressable(bool _pressable)
    {
        pressable = _pressable;
    }

    private void Event_PressableJudge()
    {
        if(Pressable == false)
        {
            TransformToTarget();
            images.Alpha = 0.5f;
        }
        else
        {
            TransformToIntial();
            images.Alpha = 1.0f;
        }
    }


    private void Pressed()
    {
        if (Pressable == false)
        {
            return;
        }


        TransformToTarget();
        isPress = true;
    }

    private void Exit()
    {
        if (Pressable == false)
        {
            return;
        }

        TransformToIntial();
        isPress = false;
    }

    private void UnPressed()
    {
        if (Pressable == false)
        {
            return;
        }

        TransformToIntial();

        if(isPress == true)
        {
            clickUpAction?.Invoke();
        }
    }


    private void TransformToIntial()
    {
        buttonImage.rectTransform.offsetMax = initialOffsetMax;
        buttonImage.rectTransform.offsetMin = initialOffsetMin;
    }

    private void TransformToTarget()
    {
        buttonImage.rectTransform.offsetMax = targetRect.offsetMax;
        buttonImage.rectTransform.offsetMin = targetRect.offsetMin;
    }
}