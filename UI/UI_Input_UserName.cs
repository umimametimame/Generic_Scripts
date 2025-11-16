using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Input_UserName : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI inputField;
    [SerializeField] private ImageButton button_OK;
    private UI_Canvas_FadeInToOut fade;

    private void Start()
    {
        fade = GetComponent<UI_Canvas_FadeInToOut>();
        button_OK.clickUpAction += Event_ClickOKButton;
    }

    private void Update()
    {
        button_OK.AssignPressable(isMeetsInputRequirements);
    }

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
            if(inputText.Length == 1)
            {
                return false;
            }

            return true;
        }
    }
    public void Event_ClickOKButton()
    {
        Rakuin_CurrentlyUserData.instance.userData.userName = inputText;
        fade.Event_LaunchFadeOut();

        Rakuin_CurrentlyUserData.instance.Event_InitialRegistration(inputText);
    }
}
