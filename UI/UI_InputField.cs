using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;
    [SerializeField] private UI_Button button_OK;
    private UI_Canvas_FadeInToOut fade;
    public Action<string> getInputStrAction { get; private set; }
    public string inputText
    {
        get
        {
            return inputField.text;
        }
    }

    public bool isMeetsInputRequirements
    {
        get
        {
            // “ü—Í‚³‚ê‚Ä‚¢‚È‚¢‚È‚ç
            if (inputText.Length == 1)
            {
                return false;
            }

            return true;
        }
    }

    private void Start()
    {
        fade = GetComponent<UI_Canvas_FadeInToOut>();
        button_OK.onClickAction += OnClickOKButton;
        button_OK.IsPressableCondition = true;
    }

    private void Update()
    {
        button_OK.AssignPressable(isMeetsInputRequirements);
    }

    public void OnClickOKButton()
    {
        fade.Event_LaunchFadeOut();
        getInputStrAction?.Invoke(inputText);

        //userData.userData.userName = inputText;
        //userData.Event_InitialRegistration(inputText);
    }

    public void Assign(Action<string> _getInputStrAction)
    {
        getInputStrAction = _getInputStrAction;
    }
}
