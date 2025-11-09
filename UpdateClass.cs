using AddUnityClass;
using System;
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

    public bool IsInRange(float value)
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

        inRange = IsInRange(currentValue);
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
    [field: SerializeField, NonEditable] public bool reaching { get; private set; }
    public float interval;
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
    /// <param name="_start"></param>
    public void Initialize(bool _start, bool _autoReset, float _interval)
    {
        if (_interval != 0.0f) 
        { 
            interval = _interval; 
        }

        autoReset = _autoReset;
        if (_start == true)
        {
            time.Initialize(_interval);
        }
        else
        {
            time.Initialize();
        }

        if(time.value >= _interval)
        {
            reaching = true;
        }
        else
        {
            reaching = false;
        }
        reached = false;
        RatioUpdate();
    }

    public void Initialize(bool _start, bool _autoReset)
    {
        Initialize(_start, _autoReset, interval);
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

            reaching = true;
            activeAction?.Invoke();
            if (autoReset == true) { Reset(); }
        }
        else
        {
            reaching = false;
            lowAction?.Invoke();
        }
    }

    public void Update()
    {
        time.Update();
        RatioUpdate();

        if (time.value >= interval)
        {
            if (reached == false)
            {
                reached = true;
                reachAction?.Invoke();
            }

            reaching = true;
            activeAction?.Invoke();
            if (autoReset == true) { Reset(); }
        }
        else
        {
            reaching = false;
            lowAction?.Invoke();
        }
    }

    /// <summary>
    /// timeを0に戻す
    /// </summary>
    public void Reset()
    {
        time.Initialize();
        reaching = (time.value >= interval) ? true : false;
        reached = false;
        RatioUpdate();
    }

    /// <summary>
    /// timeを代入する
    /// </summary>
    public void Reset(float _value)
    {
        time.Initialize(_value);
        reaching = (time.value >= interval) ? true : false;
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

    [field: SerializeField] public MinMax thresholdRange { get; private set; } = new MinMax();
    [SerializeField, NonEditable] private float currentValue;
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
        if (range != default) 
        { 
            thresholdRange = range;
        }
        Reset();
    }

    public void Reset()
    {
        beforeBool = false;
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
                withinRangeAction.Invoke();
            }

            inRangeAction?.Invoke();

        }

        if (beforeBool == true)      // 前回範囲内で
        {
            if (isReaching == false)  // 出る瞬間なら
            {
                exitRangeAction.Invoke();
            }
        }

        if (isReaching == false)   // 範囲外なら
        {
            outOfRangeAction?.Invoke();
        }

        beforeBool = isReaching;
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
    [field: SerializeField] public AnimationCurve curve { get; set; }
    public Interval time = new Interval();

    public void Initialize()
    {
        time.Initialize(false, false);
    }

    public void Update()
    {
        time.Update();
    }

    public void Reset()
    {
        time.Reset();
    }

    public float evaluate
    {
        get
        {
            return curve.Evaluate(time.ratio);
        }
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

    public void Invoke()
    {
        if (activated == false)
        {
            action?.Invoke();
            activated = true;
        }
    }

    
    public void Add(Action _action)
    {
        action += _action;
    }
}

[Serializable]
public class Shake
{
    [field: SerializeField] public Interval interval { get; set; }
    [field: SerializeField] public GameObject targetObj { get; set; }

    public void Initialize()
    {
        interval.Initialize(true, true);
    }

    public void Update()
    {
        if (interval.reaching == true)
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
    public void Update(float _value = 0.0f)
    {
        switch (increseType)
        {
            case IncreseType.DeltaTime:
                if (reversalIncrese == true) { value -= Time.deltaTime; }
                else { value += Time.deltaTime; }
                break;

            case IncreseType.Frame:
                if (reversalIncrese == true)
                {
                    value--; 
                }
                else 
                { 
                    value++; 
                }
                break;


            case IncreseType.Manual:
                value = _value;
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

    public void Update(T _value)
    {
        value = _value;
        changed = !_value.Equals(beforeValue);   // 変更されていたら

        if (changed == true)
        {
            changedAction?.Invoke();
        }

        beforeValue = value;
    }

}

[Serializable]
public class UI_ImageOperator
{
    [field: SerializeField] public GameObject target { get; private set; }
    public bool getOnlyYourself;
    public UI_Image images = new UI_Image();
    public UI_Image exclusionList = new UI_Image();
    public void Assign()
    {
        if (getOnlyYourself == true)
        {
            images.GetComponents_Sorted(exclusionList, target);
        }
        else
        {
            images.GetComponentsInChildren_Sorted(exclusionList,target);
        }
    }

    public void Assign(GameObject _obj)
    {
        if (getOnlyYourself == true)
        {
            images.GetComponents_Sorted(exclusionList, _obj);
        }
        else
        {
            images.GetComponentsInChildren_Sorted(exclusionList, _obj);
        }
    }

    public Color color
    {
        get
        {
            return images.color;
        }
        set
        {
            images.color = value;
        }
    }

    public float Alpha
    { 
        get
        {
            return images.Alpha;
        }

        set
        {
            images.Alpha = value;
        }
    }
}



/// <summary>
/// 宣言されてる型をまとめて操作する
/// </summary>
[Serializable]
public class UI_Image
{
    public List<Image> images;
    public List<SpriteRenderer> sprites;
    public List<TextMeshProUGUI> texts;

    public void Initialize()
    {

    }



    public void Add(Image _image)
    {
        images.Add(_image);
    }

    public void Add(SpriteRenderer _sprite)
    {
        sprites.Add(_sprite);
    }

    public void Add(TextMeshProUGUI _text)
    {
        texts.Add(_text);
    }

    public void GetComponents(GameObject _obj)
    {
        _obj.GetComponents<Image>().ToList();
        _obj.GetComponents<TextMeshProUGUI>().ToList();
        _obj.GetComponents<SpriteRenderer>().ToList();
    }

    public void GetComponentsInChildren(GameObject _obj)
    {
        _obj.GetComponentsInChildren<Image>().ToList();
        _obj.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        _obj.GetComponentsInChildren<SpriteRenderer>().ToList();
    }

    public void GetComponents_Sorted(UI_Image _exclusionList, GameObject _obj)
    {
        SortByExclusionList(_exclusionList,_obj.GetComponents<Image>().ToList());
        SortByExclusionList(_exclusionList,_obj.GetComponents<TextMeshProUGUI>().ToList());
        SortByExclusionList(_exclusionList,_obj.GetComponents<SpriteRenderer>().ToList());
    }
    public void GetComponentsInChildren_Sorted(UI_Image _exclusionList, GameObject _obj)
    {
        SortByExclusionList(_exclusionList, _obj.GetComponentsInChildren<Image>().ToList());
        SortByExclusionList(_exclusionList, _obj.GetComponentsInChildren<TextMeshProUGUI>().ToList());
        SortByExclusionList(_exclusionList, _obj.GetComponentsInChildren<SpriteRenderer>().ToList());
    }
    /// <summary>
    /// 後から整理する場合に使用
    /// </summary>
    public void SortByExclusionList(UI_Image _exclusionList)
    {
        for (int i = images.Count; i > 0; i--)
        {
            for (int j = 0; j < _exclusionList.images.Count; j++)
            {
                if (images[i] == _exclusionList.images[j])
                {
                    images.RemoveAt(i);
                }
            }
        }


        for (int i = sprites.Count; i > 0; i--)
        {
            for (int j = 0; j < _exclusionList.sprites.Count; j++)
            {
                if (sprites[i] == _exclusionList.sprites[j])
                {
                    sprites.RemoveAt(i);
                }
            }
        }


        for (int i = texts.Count; i > 0; i--)
        {
            for (int j = 0; j < _exclusionList.texts.Count; j++)
            {
                if (texts[i] == _exclusionList.texts[j])
                {
                    texts.RemoveAt(i);
                }
            }
        }
    }


    public void SortByExclusionList(UI_Image _exclusionList, List<Image> _imageList)
    {
        for (int i = 0; i < _imageList.Count; i++)
        {
            bool add = true;
            for (int j = 0; j < _exclusionList.images.Count; j++)
            {
                if (_imageList[i] == _exclusionList.images[j])
                {
                    add = false;
                }

            }

            if (add == true)
            {
                Add(_imageList[i]);
            }
        }
    }
    public void SortByExclusionList(UI_Image _exclusionList, List<SpriteRenderer> _spriteList)
    {
        for (int i = 0; i < _spriteList.Count; i++)
        {
            bool add = true;
            for (int j = 0; j < _exclusionList.sprites.Count; j++)
            {
                if (_spriteList[i] == _exclusionList.sprites[j])
                {
                    add = false;
                }

            }

            if (add == true)
            {
                sprites.Add(_spriteList[i]);
            }
        }
    }
    public void SortByExclusionList(UI_Image _exclusionList, List<TextMeshProUGUI> _textList)
    {
        for (int i = 0; i < _textList.Count; i++)
        {
            bool add = true;
            for (int j = 0; j < _exclusionList.texts.Count; j++)
            {
                if (_textList[i] == _exclusionList.texts[j])
                {
                    add = false;
                }

            }

            if (add == true)
            {
                texts.Add(_textList[i]);
            }
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
            if (sprites.Count != 0)
            {

                return sprites[0].color;
            }
            else if (images.Count != 0)
            {
                return images[0].color;
            }
            else if (texts.Count != 0)
            {
                return texts[0].color;
            }
            Debug.LogError("SpriteRenderer/Image/Textのいずれかをアタッチしてください");
            return Color.white;
        }
        set
        {
            if (sprites.Count != 0)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = value;

                }
            }
            else if (images.Count != 0)
            {
                foreach (Image image in images)
                {

                    image.color = value;
                }
            }
            else if (texts.Count != 0)
            {
                foreach (TextMeshProUGUI text in texts)
                {

                    text.color = value;
                }
            }
            else
            {
                Debug.LogError("SpriteRenderer/Image/Textのいずれかをアタッチしてください");
            }
        }
    }

    public float Alpha
    {
        get
        {
            if (texts.Count != 0)
            {
                return texts[0].color.a;
            }
            else if (sprites.Count != 0)
            {

                return sprites[0].color.a;
            }
            else if (images.Count != 0)
            {
                return images[0].color.a;
            }

            Debug.LogError("SpriteRenderer/Image/Textのいずれかをアタッチしてください");
            return 0.0f;
        }
        set
        {
            bool _noneImage = true;
            if (sprites.Count != 0)
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, value);

                }
                _noneImage = false;
            }
            if (images.Count != 0)
            {
                foreach (Image image in images)
                {

                    image.color = new Color(image.color.r, image.color.g, image.color.b, value);
                }
                _noneImage = false;
            }
            if (texts.Count != 0)
            {
                foreach (TextMeshProUGUI text in texts)
                {
                    text.alpha = value;
                }
                _noneImage = false;
            }

            if (_noneImage == true)
            {
                Debug.LogError("SpriteRenderer/Image/Textのいずれかをアタッチしてください");
            }
        }
    }

    public bool none
    { 
        get
        {
            if(images.Count == 0)
            {
                if(sprites.Count == 0)
                {
                    if(texts.Count == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }



}


[Serializable]
public class EnableAndFadeAlpha
{

    [field: SerializeField] public UI_ImageOperator img { get; set; } = new UI_ImageOperator();
    [SerializeField] private Interval displayInterval = new Interval(); // 消え始めるまでの時間
    [SerializeField] private Interval intervalToFade = new Interval();  // 完全に消えるまでの時間

    public void Initialize()
    {
        img.Assign();
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
    public bool reverse;
    public TextMeshProUGUI text;
    public string variableCenterText_Entity;
    public string variableCenterText_Empty;
    protected Parameter parameter = new Parameter();

    protected ValueChecker<int> valueChecker;
    public void Initialize(Parameter _startValue)
    {
        valueChecker = new ValueChecker<int>();
        parameter = _startValue;
        valueChecker.Initialize((int)parameter.entity);
        valueChecker.changedAction += Event_TextUpdate;

        Event_TextUpdate();
    }
    public void Update(int _numberOfVariable)
    {
        valueChecker.Update(_numberOfVariable);
    }
    public virtual void Event_TextUpdate()
    {
        text.text = AssignVariableCenterText;
    }

    protected string AssignVariableCenterText
    {
        get
        {
            string incrementalVariableText = "";

            if (reverse == false)
            {
                for (int i = 0; i < parameter.max; i++)
                {
                    if (i < parameter.entity)
                    {
                        incrementalVariableText += variableCenterText_Entity;
                    }
                    else
                    {
                        incrementalVariableText += variableCenterText_Empty;
                    }
                }

                return incrementalVariableText;
            }


            // 反転して表示させる
            for (int i = (int)parameter.max; i > 0; i--)
            {
                if (i <= parameter.entity)
                {
                    incrementalVariableText += variableCenterText_Entity;
                }
                else
                {
                    incrementalVariableText += variableCenterText_Empty;
                }
            }

            return incrementalVariableText;

        }
    }
}


[Serializable]
public class TextParameter_EncloseCenter : TextParameter
{
    public string firstHalfText;
    public string secondHalfText;


    public override void Event_TextUpdate()
    {
        string incrementalVariableText = "";
        for (int i = 0; i < parameter.max; i++)
        {
            if(i < parameter.entity)
            {
                incrementalVariableText += variableCenterText_Entity;
            }
            else
            {
                incrementalVariableText += variableCenterText_Empty;
            }
        }

        text.text = firstHalfText + incrementalVariableText + secondHalfText;
    }
}

[Serializable]
public class Image_IntParameter
{
    public Image entityImage;
    public Image emptyImage;

    public Parameter refferenceParameter;

}

/// <summary>
/// 慣性維持クラス
/// </summary>
[Serializable]
public class  Inertia
{
    [field: SerializeField, NonEditable] public bool active { get; private set; }
    [field: SerializeField, NonEditable] public Vector3 velocity { get; private set; }
    public Interval durationTime = new Interval();
    public RatioCurve ratioCurve = new RatioCurve();

    public void Initialize()
    {
        durationTime.Initialize(false, false, durationTime.interval);
        ratioCurve.AssignProfile();
    }

    /// <summary>
    /// 値渡しで初期化
    /// </summary>
    /// <param name=""></param>
    public void Initialize_PassByValue(Inertia _inertia)
    {
        durationTime.Initialize(false, false, _inertia.durationTime.interval);
        ratioCurve.AssignProfile();
    }

    public void Event_StartImpulse(Vector3 _velocity)
    {
        active = true;
        velocity = _velocity;
    }

    /// <summary>
    /// durationTime.intervalが0以下の場合、持続時間的に慣性を返す
    /// </summary>
    /// <returns></returns>
    public Vector3 Evaluate()
    {
        if (active == false)
        {
            return Vector3.zero;
        }



        Vector3 returnVelocity = velocity;

        // 常に同じ慣性で移動
        if (durationTime.interval <= 0)
        {
            returnVelocity *= ratioCurve.Evaluate(1);
            return returnVelocity;
        }


        durationTime.Update();
        returnVelocity *= ratioCurve.Evaluate(durationTime.ratio);
        return returnVelocity;
    }

    public void Reset()
    {
        durationTime.Reset();
        velocity = Vector3.zero;
    }

    /// <summary>
    /// durationTime.intervalが0以下なら
    /// </summary>
    public bool isInertiaDuration
    {
        get
        {
            if (durationTime.interval <= 0)
            {
                return true;
            }

            return false;
        }
    }
}