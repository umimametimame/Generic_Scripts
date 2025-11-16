using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Canvas_FadeInToOut : MonoBehaviour
{
    private CanvasGroup canvas;
    /// <summary>
    /// ì¸óÕÇµÇΩéûä‘Çä|ÇØÇƒèoåª/è¡ñ≈Ç∑ÇÈ
    /// </summary>
    public float alphaChangePerSec;
    private float alphaChangeValue;
    public bool isFadeIn;
    public bool isFadeOut;
    public MomentAction completeFadeInAction = new MomentAction();

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0.0f;
        alphaChangeValue = 1.0f / alphaChangePerSec;
    }

    private void Update()
    {
        if (isFadeIn == true)
        {
            if (isFadeOut == false)
            {
                canvas.alpha += Time.deltaTime * alphaChangeValue;
            }
        }

        else if (isFadeOut == true)
        {
            if (isFadeIn == false)
            {
                canvas.alpha -= Time.deltaTime * alphaChangeValue;
                if (canvas.alpha <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public void AddCompleteFadeInAction(Action _action)
    {
        completeFadeInAction.Add(_action);
    }

    public void Event_LaunchFadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
    }

    public void Event_LaunchFadeOut()
    {
        isFadeIn = false;
        isFadeOut = true;
    }
}
