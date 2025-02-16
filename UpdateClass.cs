using AddClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class ValueInRange
{

    public enum ThanType
    {
        ThanOrEqual,
        Than,
        LessThanOrEqual,
        Less,
    }
    public ThanType minThan;
    public MinMax range = new MinMax();
    public ThanType maxThan;
    public bool minExcess;
    public bool maxExcess;

    public bool inRange;
    public float currentValue;

    public bool JudgeRange(float value)
    {
        minExcess = false;
        switch (minThan)
        {
            case ThanType.ThanOrEqual:
                if (range.min > value)
                {
                    minExcess = true;
                }
                break;
            case ThanType.Than:
                if (range.min >= value)
                {
                    minExcess = true;
                }
                break;

            case ThanType.LessThanOrEqual:
                if (!(range.min > value))
                {

                    minExcess = true;
                }
                break;
            case ThanType.Less:
                if (!(range.min >= value))
                {
                    minExcess = true;
                }
                break;

        }


        maxExcess = false;
        switch (maxThan)
        {
            case ThanType.ThanOrEqual:
                if (value < range.max)
                {
                    maxExcess = true;
                }
                break;
            case ThanType.Than:
                if (value <= range.max)
                {
                    maxExcess = true;
                }
                break;
            case ThanType.LessThanOrEqual:
                if (value > range.max)
                {
                    maxExcess = true;
                }
                break;
            case ThanType.Less:
                if (value >= range.max)
                {
                    maxExcess = true;
                }
                break;
        }

        if (maxExcess == true || minExcess == true)
        {
            return false;
        }

        return true;
    }
    public void Update(float value = 0.0f)
    {
        if (value != 0.0f) { currentValue = value; }

        inRange = JudgeRange(currentValue);
    }

    public float min
    {
        get { return range.min; }
    }
    public float max
    {
        get { return range.max; }
    }
}

[Serializable]
public class TransformOffset
{
    [SerializeField] private Vector3 vec3Offset;
    [SerializeField] private Vector2 vec2Offset;

    public void Update(Transform transform)
    {
        transform.position += vec3Offset;
        transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);

    }
    public void Update(GameObject obj)
    {
        obj.transform.position += vec3Offset;
        obj.transform.position += new Vector3(vec2Offset.x, vec2Offset.y, 0.0f);
    }
}

public enum ExistState
{
    Disable,
    Start,
    Enable,
    Finish,
}

[Serializable]
public class Exist
{
    [field: SerializeField, NonEditable] public ExistState state { get; private set; } = ExistState.Disable;
    [field: SerializeField] public Action start { get; set; }
    [field: SerializeField] public Action enable { get; set; }
    [field: SerializeField] public Action finish { get; set; }
    [field: SerializeField] public Action disable { get; set; }


    [field: SerializeField, NonEditable] public bool oneShot { get; private set; }

    public void Initialize(bool started = false)
    {
        this.oneShot = started;
        state = ExistState.Disable;
    }

    public void Reset()
    {
        state = ExistState.Disable;
    }

    public void Update()
    {
        switch (state)
        {

            case ExistState.Start:
                state = ExistState.Enable;
                start?.Invoke();
            break;
            case ExistState.Finish:
                finish?.Invoke();
                state = ExistState.Disable;
            break;
        }

        switch(state)
        {
            case ExistState.Enable:
                enable?.Invoke();
            break;
            case ExistState.Disable:
                disable?.Invoke();
            break;
        }
    }

    public void Disable()
    {
        state = ExistState.Disable;
    }

    public void Start()
    {
        state = ExistState.Start;
    }


    /// <summary>
    /// 一度のみ
    /// </summary>
    public void StartOneShot()
    {
        if (oneShot == false)
        {
            state = ExistState.Start;
            oneShot = true;
        }
    }
    public void OneShotReset()
    {
        oneShot = false;
    }
    public void Finish()
    {
        state = ExistState.Finish;
    }
}

[Serializable]
public class MomentumPhase
{
    [field: SerializeField, NonEditable] public bool active { get; private set; }
    [field: SerializeField] public Action start { get; set; }
    [field: SerializeField] public Action enable { get; set; }
    [field: SerializeField] public Action finish { get; set; }
    [field: SerializeField] public Action disable { get; set; }
    [field: SerializeField] public ValueChecker<bool> activeChecker { get; private set; } = new ValueChecker<bool>();



    public void Initialize(bool start = false)
    {
        active = start;
        activeChecker.Initialize(start);
    }

    public void Enable()
    {
        active = true;
    }
    public void Disable()
    {
        active = false;
    }

    public void Update()
    {
        if (active == true)
        {
            if (activeChecker.beforeValue == false)
            {
                start?.Invoke();
            }

            enable?.Invoke();
        }
        else if (active == false)
        {
            if (activeChecker.beforeValue == true)
            {
                finish?.Invoke();
            }

            disable?.Invoke();
        }

        activeChecker.Update(active);
    }
}

/// <summary>
/// 移動範囲を円形に制限
/// </summary>
[Serializable]
public class CircleClamp
{

    [SerializeField] private GameObject center;
    [field: SerializeField] public GameObject moveObject { get; set; }
    [field: SerializeField] public float radius { get; private set; }
    [field: SerializeField, NonEditable] public float currentDistance { get; private set; }
    public void Initialize(GameObject center, GameObject moveObject)
    {
        this.center = center;
        this.moveObject = moveObject;
    }
    public void AdjustByCenter()
    {
        moveObject.transform.position = center.transform.position;
    }
    public void Limit()
    {
        currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);

        if (currentDistance > radius)
        {
            Debug.Log("Limitting");

            Vector3 nor = moveObject.transform.position - center.transform.position;
            moveObject.transform.position = center.transform.position + nor.normalized * radius;
            currentDistance = Vector2.Distance(moveObject.transform.position, center.transform.position);

        }

    }
}

[Serializable]
public class MoveCircleSurface
{
    [field: SerializeField] public Transform centerPos { get; set; }
    [field: SerializeField] public Transform moveObject { get; set; }
    [field: SerializeField] bool lookAtCenter { get; set; } // centerを向くか
    [field: SerializeField] public Vector3 axis { get; set; }   // transform.rightなどで代入する
    [SerializeField] private float distanceFromCenter;
    [SerializeField, NonEditable] private Vector3 norAxis;
    [SerializeField, NonEditable] private Quaternion angleAxis;
    [SerializeField] private float speed;
    [field: SerializeField, NonEditable] public Vector3 angleFromCenter { get; private set; }
    public void Initialize(GameObject moveObject)
    {
        this.moveObject = moveObject.transform;
    }
    public void Initialize(Transform moveObject)
    {
        this.moveObject = moveObject;
    }

    public void SetDistance(Vector3 axis)
    {
        moveObject.position = centerPos.position + (axis * distanceFromCenter);
    }

    /// <summary>
    /// 引数は回転スピード
    /// </summary>
    /// <param name="speed"></param>
    public void Update(float speed)
    {
        if (speed == 0.0f) { return; }
        this.speed = speed;

        norAxis = axis.normalized;
        angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

        moveObject.position -= centerPos.position;
        moveObject.position = angleAxis * moveObject.position;
        moveObject.position += centerPos.position;

        if (lookAtCenter == true)
        {
            moveObject.rotation = moveObject.rotation * angleAxis;
        }

        angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

    }
    public Vector3 NewPosUpdate(float speed)
    {
        if (speed == 0.0f) { return Vector3.zero; }
        this.speed = speed;

        norAxis = axis.normalized;
        angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

        Vector3 newPos;

        newPos = moveObject.position - centerPos.position;
        newPos = angleAxis * newPos;
        newPos += centerPos.position;

        if (lookAtCenter == true)
        {
            moveObject.rotation = moveObject.rotation * angleAxis;
        }

        angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);

        return newPos;
    }
    public void Update()
    {
        if (this.speed == 0.0f) { return; }

        norAxis = axis.normalized;
        angleAxis = Quaternion.AngleAxis(360 * this.speed * Time.deltaTime, norAxis);

        moveObject.position -= centerPos.position;
        moveObject.position = angleAxis * moveObject.position;
        moveObject.position += centerPos.position;

        if (lookAtCenter == true)
        {
            moveObject.rotation = moveObject.rotation * angleAxis;
        }

        angleFromCenter = AddFunction.Vector3AngleSet(centerPos.position, moveObject.position);
    }


}

#region クラスの実行タイプ
/// <summary>
/// boolで判断し、trueの場合にメインの処理を行う
/// </summary>
[Serializable]
public class Traffic
{
    public bool active;
    public Action launchAction { get; set; }
    public Action enableAction { get; set; }
    public Action disableAction { get; set; }
    public Action closeAction { get; set; }
    public void Initialize()
    {
        active = false;
        enableAction = null;
        disableAction = null;
    }
    public void Update()
    {
        if (active == true)
        {
            enableAction?.Invoke();
        }
        else
        {
            disableAction?.Invoke();
        }
    }

    public void Launch()
    {
        active = true;
        launchAction?.Invoke();
    }
    public void Close()
    {
        active = false;
        closeAction?.Invoke();
    }

}

/// <summary>
/// 間隔制御クラス
/// </summary>
[Serializable]
public class Interval
{
    [field: SerializeField, NonEditable] public bool active { get; private set; }
    [field: SerializeField] public float interval { get; private set; }
    [field: SerializeField, NonEditable] public VariedTime time { get; private set; } = new VariedTime();
    [field: SerializeField, NonEditable] public float ratio { get; private set; }
    private bool autoReset;
    private bool reached;
    public Action reachAction { get; set; }
    public Action activeAction { get; set; }
    public Action lowAction { get; set; }

    /// <summary>
    /// 引数:<br/>
    /// ・最初からactiveにする(interval値とvalueを同じにする)か<br/>
    /// ・valueがinterval値に到達したら0に戻るか<br/>
    /// ・最初のinterval値
    /// </summary>
    /// <param name="start"></param>
    public void Initialize(bool start, bool autoReset = true, float _interval = 0.0f)
    {
        if (_interval != 0.0f) { this.interval = _interval; }
        this.autoReset = autoReset;
        if (start == true)
        {
            time.Initialize(_interval);
        }
        else
        {
            time.Initialize();
        }

        active = (time.value >= _interval) ? true : false;
        reached = false;
        RatioUpdate();
    }


    public void Update(float manualValue = 0.0f)
    {
        time.Update(manualValue);
        RatioUpdate();

        if (time.value >= interval)
        {
            if (reached == false)
            {
                reached = true;
                reachAction?.Invoke();
            }

            active = true;
            activeAction?.Invoke();
            if (autoReset == true) { Reset(); }
        }
        else
        {
            active = false;
            lowAction?.Invoke();
        }
    }

    /// <summary>
    /// timeを0に戻す
    /// </summary>
    public void Reset()
    {
        time.Initialize();
        active = (time.value >= interval) ? true : false;
        reached = false;
        RatioUpdate();
    }

    /// <summary>
    /// timeを代入する
    /// </summary>
    public void Reset(float _value)
    {
        time.Initialize(_value);
        active = (time.value >= interval) ? true : false;
        reached = false;
        RatioUpdate();
    }
    public void Increse(float value)
    {
        time.Increse(value);
    }

    private void RatioUpdate()
    {
        if (interval <= 0.0f)
        {
            ratio = 0;
        }
        else
        {
            ratio = time.value / interval;
        }

    }

}

/// <summary>
/// 範囲毎にActionを実行する
/// </summary>
[Serializable]
public class Range
{
    [field: SerializeField, NonEditable] public bool isReaching { get; private set; }
    private bool beforeBool;

    [SerializeField] private MinMax thresholdRange = new MinMax();
    [SerializeField, NonEditable] private float currentValue;
    [SerializeField, NonEditable] private MinMax beforeRange = new MinMax();
    public MomentAction withinRangeAction { get; set; } = new MomentAction();   // 範囲内に入る時に一度行われる
    public Action inRangeAction { get; set; }                                   // 範囲内に入っている間に行われる
    public MomentAction exitRangeAction { get; set; } = new MomentAction();     // 範囲外に出る時に一度行われる
    public Action outOfRangeAction { get; set; }                                // 範囲外に出ている間に行われる

    public void Initialize(float min, float max)
    {
        thresholdRange.Initialize(min, max);
        Reset();
    }
    public void Initialize(MinMax range = default)
    {
        if (range != default) { thresholdRange = range; }
        Reset();
    }

    public void Reset()
    {
        beforeBool = false;
        beforeRange.Clear();
        withinRangeAction.Initialize();
        exitRangeAction.Initialize();
    }

    /// <summary>
    /// 引数:現在の割合
    /// </summary>
    /// <param name="value"></param>
    public void Update(float value)
    {
        currentValue = value;

        // 範囲内なら
        if (thresholdRange.min <= currentValue && currentValue <= thresholdRange.max)
        { 
            isReaching = true; 
        }
        else 
        { 
            isReaching = false; 
        }


        if (isReaching == true)        // 範囲内で
        {
            if (beforeBool == false)    // 入った瞬間なら
            {
                withinRangeAction.Enable();
            }

            inRangeAction?.Invoke();

        }

        if (beforeBool == true)      // 前回範囲内で
        {
            if (isReaching == false)  // 出る瞬間なら
            {
                exitRangeAction.Enable();
            }
        }

        if (isReaching == false)   // 範囲外なら
        {
            outOfRangeAction?.Invoke();
        }

        beforeBool = isReaching;
        beforeRange = thresholdRange;
    }

    public bool IsReaching
    {
        get
        {
            if (thresholdRange.min <= currentValue && currentValue <= thresholdRange.max)
            {
                return true;
            }
            return false;

        }
    }

}
#endregion

[Serializable]
public class SmoothRotate
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject targetObj;
    public void Initialize(GameObject targetObj)
    {
        this.targetObj = targetObj;
    }
    public void Update(Vector3 direction)
    {
        if (direction == Vector3.zero) { return; }
        Quaternion me = targetObj.transform.rotation;
        Quaternion you = Quaternion.LookRotation(direction);
        targetObj.transform.rotation = Quaternion.RotateTowards(me, you, speed * Time.deltaTime);
    }
    public void Update(Transform changeTra)
    {
        Quaternion me = changeTra.rotation;
        Quaternion you = Quaternion.LookRotation(changeTra.position - targetObj.transform.position);
        targetObj.transform.rotation = Quaternion.RotateTowards(me, you, speed * Time.deltaTime);
    }
}

/// <summary>
/// AnimatorをEasingで制御できる
/// </summary>
[Serializable]
public class Easing
{
    [SerializeField] private Traffic traffic;
    [field: SerializeField, NonEditable] public float nowTime { get; private set; }
    [field: SerializeField, NonEditable] public float evaluteValue { get; private set; }
    [field: SerializeField] public AnimationCurve curve { get; set; }

    public void Initialize()
    {
        Reset();

        traffic.Initialize();
        traffic.enableAction += Evalute;
        traffic.disableAction += Reset;
    }
    public void Update()
    {
        traffic.Update();
    }

    public void Reset()
    {
        nowTime = 0.0f;
        evaluteValue *= curve.Evaluate(nowTime);
    }
    public void Evalute()
    {
        evaluteValue = curve.Evaluate(nowTime);
        nowTime += Time.deltaTime;
    }

    public bool active
    {
        get { return traffic.active; }
        set { traffic.active = value; }
    }
}

/// <summary>
/// AnimationをEasing割合で制御できる
/// </summary>
[Serializable]
public class EasingAnimator
{
    [SerializeField] private Traffic traffic;
    [field: SerializeField, NonEditable] public float nowRatio { get; private set; }
    [field: SerializeField, NonEditable] public float maxTime { get; private set; }
    [SerializeField] private AnimationCurve curve;
    public Animator animator { get; set; }

    public void Initialize(float maxTime, Animator animator = null)
    {
        if (animator != null) { this.animator = animator; }
        this.maxTime = maxTime;
        nowRatio = 0.0f;


        traffic.Initialize();
        traffic.enableAction += Evalute;
        traffic.disableAction += Reset;
    }
    public void Update()
    {
        traffic.Update();
    }

    public void Reset()
    {
        nowRatio = 0.0f;
    }
    public void Evalute()
    {
        animator.speed = curve.Evaluate(nowRatio);
        nowRatio += 1 / maxTime * Time.deltaTime;
    }

    public bool active
    {
        get { return traffic.active; }
        set { traffic.active = value; }
    }
}



/// <summary>
/// Update内でも一度だけ実行できる
/// </summary>
[Serializable]
public class MomentAction
{
    public Action action { get; set; }
    [SerializeField, NonEditable] private bool activated;

    public void Initialize()
    {
        activated = false;
    }

    public void Enable()
    {
        if (activated == false)
        {
            action?.Invoke();
            activated = true;
        }
    }
}

[Serializable]
public class Shake
{
    [field: SerializeField] public Interval interval { get; set; }
    [field: SerializeField] public GameObject targetObj { get; set; }

    public void Initialize()
    {
        interval.Initialize(true);
    }

    public void Update()
    {
        if (interval.active == true)
        {
        }
        interval.Update();

    }

}
[Serializable]
public class VariedTime
{
    [field: SerializeField, NonEditable] public float value { get; private set; }
    [field: SerializeField] public IncreseType increseType { get; set; }
    [SerializeField] private bool reversalIncrese;
    public void Initialize(float startTime = 0.0f, IncreseType type = default)
    {
        value = startTime;
        if (type != default) { increseType = type; }
    }
    public void Update(float value = 0.0f)
    {
        switch (increseType)
        {
            case IncreseType.DeltaTime:
                if (reversalIncrese == true) { this.value -= Time.deltaTime; }
                else { this.value += Time.deltaTime; }
                break;

            case IncreseType.Frame:
                if (reversalIncrese == true) { this.value++; }
                else { this.value--; }
                break;

            case IncreseType.Manual:
                this.value = value;
                break;
        }

    }

    public void Increse(float _value)
    {
        value += _value;
    }
}



[Serializable]
public class ValueChecker<T> where T : struct
{
    [field: SerializeField, NonEditable] public T value { get; private set; }
    [field: SerializeField, NonEditable] public T beforeValue { get; private set; }
    [field: SerializeField, NonEditable] public bool changed { get; private set; }
    public Action changedAction { get; set; }

    public void Initialize(T value)
    {
        Reset(value);
        changedAction = null;
    }

    public void Reset(T value)
    {
        this.value = value;
        beforeValue = value;
        changed = false;
    }

    public void Update(T value)
    {
        this.value = value;
        changed = !value.Equals(beforeValue);   // 変更されていたら

        if (changed == true)
        {
            changedAction?.Invoke();
        }

        beforeValue = value;
    }
}


/// <summary>
/// SpriteRenderer,Image,TMProのすべてを取得する
/// </summary>
[Serializable]
public class SpriteOrImage
{
    [field: SerializeField] public GameObject target { get; set; }
    [field: SerializeField] public SpriteRenderer[] sprites { get; set; }
    [field: SerializeField] public Image[] images { get; set; }
    [field: SerializeField] public TextMeshProUGUI[] texts { get; set; }

    /// <summary>
    /// 引数はSpriteRendererまたはImageがアタッチされたGameObject
    /// </summary>
    /// <param name="obj"></param>
    public void Initialize(GameObject obj = null)
    {
        if (obj != null)
        {
            sprites = obj.GetComponentsInChildren<SpriteRenderer>();
            images = obj.GetComponentsInChildren<Image>();
            texts = obj.GetComponentsInChildren<TextMeshProUGUI>();
        }
        else
        {
            sprites = target.GetComponentsInChildren<SpriteRenderer>();
            images = target.GetComponentsInChildren<Image>();
            texts = target.GetComponentsInChildren<TextMeshProUGUI>();

        }
        if (sprites.Length == 0 && images.Length == 0 && texts.Length == 0)
        {
            Debug.LogError("いずれもアタッチされていません");
        }
    }

    /// <summary>
    /// SpriteRendererまたはImageのColorを返す<br/>
    /// 両方存在しない場合はエラーメッセージを出してColor.whiteを返す
    /// </summary>
    public Color color
    {
        get
        {
            if (sprites.Length != 0)
            {

                return sprites[0].color;
            }
            else if (images.Length != 0)
            {
                return images[0].color;
            }
            else if (texts.Length != 0)
            {
                return texts[0].color;
            }
            Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
            return Color.white;
        }
        set
        {
            if (sprites.Length != 0)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = value;

                }
            }
            else if (images.Length != 0)
            {
                foreach (Image image in images)
                {

                    image.color = value;
                }
            }
            else if (texts.Length != 0)
            {
                foreach (TextMeshProUGUI text in texts)
                {

                    text.color = value;
                }
            }
            else
            {
                Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
            }
        }
    }

    public float Alpha
    {
        get
        {
            if (sprites.Length != 0)
            {

                return sprites[0].color.a;
            }
            else if (images.Length != 0)
            {
                return images[0].color.a;
            }
            else if (texts.Length != 0)
            {
                return texts[0].color.a;
            }

            Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
            return 0.0f;
        }
        set
        {
            if (sprites.Length != 0)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);

                }
            }
            else if (images.Length != 0)
            {
                foreach (Image image in images)
                {

                    image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                }
            }
            else if (texts.Length != 0)
            {
                foreach (TextMeshProUGUI text in texts)
                {

                    text.color = new Color(text.color.r, text.color.g, text.color.b, value);
                }
            }
            else
            {
                Debug.LogError("SpriteRendererまたはImageをアタッチしてください");
            }
        }
    }
}
[Serializable]
public class EnableAndFadeAlpha
{

    [field: SerializeField] public SpriteOrImage img { get; set; } = new SpriteOrImage();
    [SerializeField] private Interval displayInterval = new Interval(); // 消え始めるまでの時間
    [SerializeField] private Interval intervalToFade = new Interval();  // 完全に消えるまでの時間

    public void Initialize()
    {
        img.Initialize();
        displayInterval.Initialize(false, false);
        intervalToFade.Initialize(false, false);
        displayInterval.lowAction += intervalToFade.Reset;
        intervalToFade.lowAction += Fade;
        Reset();
    }

    public void Update()
    {
        displayInterval.Update();
        intervalToFade.Update();
    }

    public void Launch()
    {
        displayInterval.Reset();
        intervalToFade.Reset();
        img.Alpha = 1.0f;
        Debug.Log(img.Alpha);
    }

    public void Reset()
    {
        img.Alpha = 0.0f;
        Debug.Log(img.Alpha);
    }

    public void Fade()
    {
        img.Alpha = 1.0f - intervalToFade.ratio;
    }

    public GameObject obj
    {
        get { return img.target; }
    }
}


[Serializable]
public class TargetColliders<T> where T : class
{
    [field: SerializeField] public List<T> targets { get; set; } = new List<T>();
    public Action firstTimeAction { get; set; }
    public TargetColliders()
    {
        Clear();
    }

    public void Clear()
    {
        targets.Clear();
    }

    public void Update(T you)
    {
        bool firstTime = true;

        foreach (T t in targets)  // targetsをループして
        {
            if (t == you)
            {
                firstTime = false;
            }
        }



        if (firstTime == true)          // 同一個体でなければ
        {                               // targetsに追加する
            targets.Add(you);
            firstTimeAction?.Invoke();
        }
    }

    /// <summary>
    /// 引数に対応する配列Indexを返す
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public int GetIndex(T t)
    {
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == t) { return i; }

        }

        return -1;
    }
    public int Count
    {
        get { return targets.Count; }
    }

    public List<T> List
    {
        get { return targets; }
    }

}






[Serializable]
public class Curve
{
    [SerializeField] private AnimationCurve curve = new AnimationCurve();
    [SerializeField] private VariedTime time = new VariedTime();
    [field: SerializeField] public float currentValue { get; private set; }
    public Curve()
    {
        time = new VariedTime();
    }
    public float Update()
    {
        currentValue = curve.Evaluate(time.value);

        time.Update();

        return currentValue;
    }

    public void Clear()
    {
        time.Initialize();
    }
}



[Serializable]
public class Vec3Curve
{
    public AnimationCurve xCurve;
    public AnimationCurve yCurve;
    public AnimationCurve zCurve;
    private List<AnimationCurve> curves = new List<AnimationCurve>();

    public void Initialize()
    {
        Reset();
    }

    public void Reset()
    {

        curves.Clear();
        curves.Add(xCurve);
        curves.Add(yCurve);
        curves.Add(zCurve);

    }

    public void ZeroFill()
    {
        for (int i = 0; i < curves.Count; ++i)
        {
            bool artificial = false;
            if (curves[i].length != 0 && curves[i].length != 1)
            {
                artificial = true;
            }

            if (artificial == false)
            {
                for (int j = 0; j < curves[i].length; ++j)
                {

                    curves[i].RemoveKey(j);
                }

                curves[i].AddKey(0.0f, 0.0f);
                curves[i].AddKey(1.0f, 0.0f);

                for (int j = 0; j < curves[i].length; ++j)
                {
                    curves[i].MoveKey(j, new Keyframe(j, 0));
                }

                curves[i].postWrapMode = WrapMode.Loop;
                curves[i].preWrapMode = WrapMode.Loop;
            }
        }
    }
    public void Clear()
    {
        for (int i = 0; i < curves.Count; ++i)
        {
            for (int k = 0; k < 3; ++k)
            {
                for (int j = 0; j < curves[i].length; ++j)
                {
                    curves[i].MoveKey(j, new Keyframe(0, 0));
                    curves[i].RemoveKey(0);
                }

            }
        }
    }

    /// <summary>
    /// 現在時間
    /// </summary>
    public Vector3 Eva(float time)
    {
        Vector3 newEva;
        newEva.x = xCurve.Evaluate(time);
        newEva.y = yCurve.Evaluate(time);
        newEva.z = zCurve.Evaluate(time);

        return newEva;
    }

}

/// <summary>
/// 図形のlocalScale.xまたはyを参照値に合わせて拡縮させる
/// </summary>
[Serializable]
public class BarByParam
{

    [SerializeField] private GameObject bar;
    [SerializeField] private float entity;
    [SerializeField] private float max;
    [SerializeField] private float ratio;
    [SerializeField] private bool warp;

    public void Update(float entity, float max)
    {
        this.entity = entity;
        this.max = max;
        ratio = entity / max;
        if (warp == true)
        {
            bar.transform.localScale = new Vector3(bar.transform.localScale.x, ratio);

        }
        else
        {
            bar.transform.localScale = new Vector3(ratio, bar.transform.localScale.y);


        }
    }
}


public class AnimatorParameter
{
    public AnimatorControllerParameter MotionState { get; private set; } = new AnimatorControllerParameter();
    public AnimatorControllerParameter MoveInput { get; private set; } = new AnimatorControllerParameter();

    public AnimatorParameter(Animator _animator)
    {
        List<AnimatorControllerParameter> aniParamList = new List<AnimatorControllerParameter>();
        aniParamList = _animator.parameters.ToList();

        List<GeneralMotion> stateList = new List<GeneralMotion>();
        stateList = GetEnumList.Get<GeneralMotion>();

        // 宣言済みのAnimatorControllerParameterを初期化
        MotionState.type = AnimatorControllerParameterType.Int;
        MotionState.name = nameof(MotionState);
        aniParamList.Add(MotionState);


        MoveInput.type = AnimatorControllerParameterType.Bool;
        MoveInput.name = nameof(MoveInput);
        aniParamList.Add(MoveInput);

        // RigorMotionを追加
        for (int i = 0; i < stateList.Count; i++)
        {

            // 単発系モーションなら
            if ((int)stateList[i] >= 10)
            {
                AnimatorControllerParameter addParam = new AnimatorControllerParameter();
                addParam.type = AnimatorControllerParameterType.Trigger;
                addParam.name = stateList[i].ToString() + "Similar";

                aniParamList.Add(addParam);
            }
        }


        for (int i = 0; i < aniParamList.Count; i++)
        {
            Debug.Log(aniParamList[i].name);
        }
        var controller = new AnimatorController();
        controller.AddParameter("ParameterId", AnimatorControllerParameterType.Int);
        controller.AddLayer("Base Layer");
        var layer = controller.layers[0];
        var stateMachine = layer.stateMachine;

        // State 追加
        var state = stateMachine.AddState("State1");

        // Transition 追加
        var transition = stateMachine.AddAnyStateTransition(state);

        // Condition 追加はそのままで OK
        transition.AddCondition(AnimatorConditionMode.Equals, 1, "ParameterId");
    }
}



[Serializable]
public class TextParameter
{
    public TextMeshProUGUI text;
    public string firstHalfText;
    public string secondHalfText;
    public string variableCenterText;

    [SerializeField] private ValueChecker<int> valueChecker;
    public void Initialize(int _startValue)
    {
        valueChecker = new ValueChecker<int>();
        valueChecker.Initialize(_startValue);
        valueChecker.changedAction += TextUpdate;

        TextUpdate();
    }

    public void Update(int _numberOfVariable)
    {
        valueChecker.Update(_numberOfVariable);
    }

    public void TextUpdate()
    {

        string incrementalVariableText = "";
        for (int i = 0; i < valueChecker.value; i++)
        {
            incrementalVariableText += variableCenterText;
        }

        text.text = firstHalfText + incrementalVariableText + secondHalfText;
    }
}