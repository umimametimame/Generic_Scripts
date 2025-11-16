using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    private EventTrigger eventTrigger;
    public Action onClickAction
    {
        get
        {
            return imageButton.onClickAction;
        }
        set
        {
            imageButton.onClickAction = value;
        }
    }
    [field: SerializeField, NonEditable] public ImageButton imageButton { get; protected set; }
    public string text
    {
        get
        {
            return imageButton.text.text;
        }
        set
        {
            imageButton.text.text = value;
        }

    }
    public ColorProfile_Assign textColor
    {
        get
        {
            return imageButton.textColor;
        }
        set
        {
            imageButton.textColor = value;
        }
    }
    [field: SerializeField, NonEditable] public Image buttonImage {  get; protected set; }
    [field: SerializeField] public List<ColorProfile_Assign> figureColors { get; set; } = new List<ColorProfile_Assign>();
    [field: SerializeField, NonEditable] public bool isPress { get; protected set; }

    protected virtual void Awake()
    {
        isPress = false;

        eventTrigger = GetComponent<EventTrigger>();
        eventTrigger.AddPointerClickListener(_ =>onClickAction?.Invoke());

        imageButton = GetComponentInChildren<ImageButton>();
        buttonImage = imageButton.GetComponent<Image>();
        imageButton.onPressableChange += Event_PressableChange;
    }

    private void Start()
    {
        
    }

    public void AssignColorProfile(ColorProfile _colorProfile)
    {
        for (int i = 0; i < figureColors.Count; i++)
        {
            if (figureColors[i].adjustAlphaProfile != null)
            {
                figureColors[i].adjustAlphaProfile = _colorProfile;
            }
            if (figureColors[i].colorProfile != null)
            {
                figureColors[i].Assign_ColorProfile(_colorProfile);
            }

        }
    }

    private void Event_PressableChange(bool _pressable)
    {

        if (_pressable == false)
        {
            Event_UnPressable();
        }
        else
        {
            Event_Pressable();
        }
    }

    public virtual void Event_Pressable()
    {

    }

    public virtual void Event_UnPressable()
    {

    }

    public void AssignPressable(bool _pressable)
    {
        imageButton.AssignPressable(_pressable);
    }

    public bool IsPressableCondition
    {
        get
        {
            return imageButton.IsPressableCondition;
        }
        set
        {
            imageButton.IsPressableCondition = value;
        }
    }
}
