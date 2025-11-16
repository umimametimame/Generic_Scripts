using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AlphaValueFlash : MonoBehaviour
{
    [field: SerializeField] public bool enable { get; private set; }
    /// <summary>
    /// imageの最初の透明度
    /// </summary>
    private float initialAlpha;
    public MinMax alphaValue = new MinMax();
    public UI_ImageOperator image = new UI_ImageOperator();
    private RegisterEventTrigger trigger;
    /// <summary>
    /// intervalが負の場合ループする<br/>
    /// intervalが自然数かつ奇数の場合初期値に戻る
    /// </summary>
    public Interval flashCount = new Interval();
    public Interval interval = new Interval();
    private Interval intervalByloop = new Interval();

    private void Start()
    {

        trigger = GetComponent<RegisterEventTrigger>();
        trigger.onEnter += Event_Launch;
        trigger.onExit += Event_Stop;

        image.Assign(gameObject);
        flashCount.Initialize(false, false);
        interval.Initialize(false, true);
        intervalByloop.Initialize(false, true);

        interval.reachAction += flashCount.Update;
        flashCount.lowAction += Event_ReverseImageAlpha;
        intervalByloop.reachAction += Event_ReverseImageAlpha;

        initialAlpha = image.Alpha;
    }

    private void FixedUpdate()
    {
        if(enable == false)
        {
            return;
        }


        if(flashCount.interval < 0)
        {
            intervalByloop.Update();
        }
        else
        {

            interval.Update();
        }
    }


    public void Event_ReverseImageAlpha()
    {
        if(image.Alpha > alphaValue.half)
        {
            image.Alpha = alphaValue.min;
        }
        else
        {
            image.Alpha = alphaValue.max;
        }
    }

    public void Assign()
    {

    }

    public void Event_Launch()
    {
        flashCount.Reset();
        interval.Reset();
        intervalByloop.Reset();
        enable = true;
    }

    public void Event_Stop()
    {
        if (image.none == true)
        {
            return;
        }

        image.Alpha = initialAlpha;
        enable = false;
    }


    private void OnDisable()
    {
        Event_Stop();
    }
}
