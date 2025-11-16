using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorProfile_Assign_UI : ColorProfile_Assign
{
    public ColorProfile_UI profile;
    public UITag colorTag;
    
    public override void Assign()
    {
        if(profile == null)
        {
            return;
        }


        if(image != null)
        {
            UITag i = IsMatchEnum(colorTag);
            image.color = profile.colors[i].color;
        }

        if(text != null)
        {
            UITag i = IsMatchEnum(colorTag);
            text.color = profile.colors[i].color;
        }
    }

}
public enum UITag
{
    Normal_Panel,
    Normal_Text,
}
