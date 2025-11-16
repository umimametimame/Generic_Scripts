using AddUnityClass;
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Editorägí£
/// </summary>
[ExecuteAlways]
public class ColorProfile_Assign : MonoBehaviour
{
    [SerializeField] protected UI_ImageOperator image = new UI_ImageOperator();
    public ColorProfile colorProfile;
    public ColorProfile adjustAlphaProfile;
    public List<ColorProfile> changeColorByEvent = new List<ColorProfile>();
    private void Start()
    {
        Assign_Image();
        Assign_Color();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Application.isPlaying == true)
        {
            return;
        }

        if (image != null)
        {
            Assign_Color();
        }
#endif

    }

    public void Assign_ColorProfile(ColorProfile _profile)
    {
        colorProfile = _profile;
        Assign_Color();
    }

    /// <summary>
    /// changeColorByEventÇÃóvëfêîÇ™1ÇÃèÍçáÇ…égóp
    /// </summary>
    public void Event_ChangeColor()
    {
        Event_ChangeColor(0);
    }

    public void Event_ChangeColor(int _index)
    {
        image.color = changeColorByEvent[_index].color;
    }

    public virtual void Assign_Image()
    {
        if (colorProfile == null)
        {
            return;
        }
        image.getOnlyYourself = true;
        image.Assign(gameObject);
    }

    [ContextMenu(nameof(Assign_Color))]
    public void Assign_Color()
    {
        if (colorProfile == null)
        {
            return;
        }

        image.color = colorProfile.color;

        if (adjustAlphaProfile != null)
        {
            image.color *= adjustAlphaProfile.color;
        }
    }

    private void OnValidate()
    {
        Assign_Image();
    }

    public bool IsHaveChangeColor
    {
        get
        {
            if (changeColorByEvent.Count > 0)
            {
                return true;
            }

            return false;
        }
    }

    private void OnEnable()
    {
        Assign_Color();
    }
}