using AddUnityClass;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Text_Easing : MonoBehaviour
{
    [field: SerializeField] public TextMeshProUGUI text { get; private set; }
    public Easing easing;
    private float targetCharacterSpacint;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        targetCharacterSpacint = text.characterSpacing;
        Event_Launch();
    }

    private void Update()
    {
        easing.Update();
        text.characterSpacing = easing.evaluate * targetCharacterSpacint;
    }

    public void Event_Launch()
    {
        easing.Reset();
    }
}

