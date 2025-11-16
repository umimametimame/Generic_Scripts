using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour
{
    [SerializeField] private UI_ImageOperator images = new UI_ImageOperator();
    public ColorProfile_Assign imageColor;
    public TextMeshProUGUI text;
    public ColorProfile_Assign textColor;
    public RegisterEventTrigger trigger;
    /// <summary>
    /// ˆø”‚Í‰Ÿ‚¹‚éê‡
    /// </summary>
    public Action<bool> onPressableChange;
    public Action onPressed;
    public Action onPressingToRelease;
    /// <summary>
    /// ƒ{ƒ^ƒ“‚ª‰Ÿ‚¹‚È‚¢ê‡‚ª‘¶İ‚·‚é‚©
    /// </summary>
    public bool IsPressableCondition;
    private bool pressable;
    public bool Pressable
    {
        get
        {
            if(IsPressableCondition == true)
            {
                return pressable;
            }

            return true;
        }
    }
    public ValueChecker<bool> pressableCheck;
    /// <summary>
    /// ³‹K‚Ìè‡‚Å‰Ÿ‚µ‚½“®ì
    /// </summary>
    public Action onClickAction;

    private void Awake()
    {
        imageColor = GetComponent<ColorProfile_Assign>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        textColor = text.GetComponent<ColorProfile_Assign>();
    }

    private void Start()
    {
        images.Assign(gameObject);



        pressableCheck.Initialize(Pressable);
        pressableCheck.changedAction += Event_PressableChange;
        Event_PressableChange();
    }

    public void Update()
    {
        if(IsPressableCondition == true)
        {
            pressableCheck.Update(Pressable);

        }
    }

    public void AssignPressable(bool _pressable)
    {
        pressable = _pressable;
    }

    private void Event_PressableChange()
    {
        if(Pressable == false)
        {
            images.Alpha = 0.5f;
        }
        else
        {
            images.Alpha = 1.0f;
        }
        onPressableChange?.Invoke(Pressable);
    }




}