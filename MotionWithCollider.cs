using AddClass;
using System;
using UnityEngine;
using GenericChara;
[Serializable] public class MotionWithCollider
{
    [SerializeField] MotionCollider motionCol;
    private bool MotionColPassingFunc(bool passing, Collider you)
    {
        if (!(you.tag == Tags.Player01 || you.tag == Tags.Player02)) { passing = false; }
        return passing;
    }
    [field: SerializeField] public Motion motion { get; set; }

    [SerializeField, NonEditable] private float damage;
    [SerializeField, NonEditable] private int hitCount;

    public void Initialize(Animator animator, string clipName, Chara parent)
    {
        motion.Initialize(animator, clipName);

        Reset();

        motionCol.Initialize();
        motionCol.parent = parent;
        motionCol.passJudgeFunc = null;
        motionCol.passJudgeFunc += MotionColPassingFunc;

        withinThreshold += () => motionCol.Launch(damage, hitCount);
        endAction += motionCol.Spawn;
        cutIn += motionCol.Spawn;
    }

    /// <summary>
    /// 引数:<br/>
    /// ・ダメージ<br/>
    /// ・ヒット回数
    /// </summary>
    public void Launch(float damage, int hitCount = 1)
    {
        motion.Launch();
        this.damage = damage;
        this.hitCount = hitCount;
    }

    public void Update()
    {
        motion.Update();
    }

    public void Reset()
    {
        motion.Reset();
        motionCol.Spawn();

        damage = 0.0f;
        hitCount = 0;
    }
    public Action cutIn
    {
        get { return motion.cutIn; }
        set { motion.cutIn = value; }
    }

    #region Existプロパティ

    public Exist exist
    {
        get { return motion.exist; }
        set { motion.exist = value; }
    }
    public Action startAction
    {
        get { return exist.start; }
        set { exist.start = value; }
    }

    public Action enableAction
    {
        get { return exist.enable; }
        set { exist.enable = value; }
    }
    public Action endAction
    {
        get { return interval.activeAction; }
        set { interval.activeAction = value; }
    }
    #endregion

    #region Thresholdプロパティ
    public Interval interval
    {
        get { return motion.interval; }
    }

    public Range motionThreshold
    {
        get { return motion.motionThreshold; }
    }
    public Action withinThreshold
    {
        get { return motionThreshold.withinRangeAction.action; }
        set { motionThreshold.withinRangeAction.action = value; }
    }

    public Action inThreshold
    {
        get { return motionThreshold.inRangeAction; }
        set { motionThreshold.inRangeAction = value; }
    }

    public Action exitThreshold
    {
        get { return motionThreshold.exitRangeAction.action; }
        set { motionThreshold.exitRangeAction.action = value; }
    }

    public Action outThreshold
    {
        get { return motionThreshold.outOfRangeAction; }
        set { motionThreshold.outOfRangeAction = value; }
    }
    #endregion

}


[Serializable]　public class Motion
{
    public Action cutIn { get; set; }
    [SerializeField, NonEditable] private float motionTime;
    [field: SerializeField] public Interval interval { get; set; }
    [field: SerializeField] public Range motionThreshold { get; set; }
    [field: SerializeField] public Exist exist { get; set; } = new Exist();
    [field: SerializeField] public EasingAnimator easAnim { get; private set; } = new EasingAnimator();
    public void Initialize(Animator animator, string clipName)
    {
        motionTime = AddFunction.GetAnimationClipLength(animator, clipName);


        exist.start += () => easAnim.Initialize(motionTime, animator);
        exist.start += easAnim.Reset;
        exist.start += () => interval.Initialize(false, true, motionTime);
        exist.start += motionThreshold.Reset;

        exist.enable += easAnim.Update;
        
        exist.enable += () => interval.Update();
        exist.enable += () => motionThreshold.Update(easAnim.nowRatio);

        exist.finish += Reset;

        interval.activeAction += exist.Finish;
        interval.activeAction += exist.Reset;
    }


    public void Reset()
    {
        easAnim.Reset();
        exist.Reset();
        interval.Initialize(false, true, motionTime);
        motionThreshold.Reset();

    }

    public void Update()
    {
        exist.Update();
        easAnim.active = true;
    }

    public void Launch()
    {
        exist.Start();
    }
    public void LaunchOneShot()
    {
        exist.StartOneShot();
    }

    public bool active
    {
        get
        {
            return !interval.active;
        }
    }
    public float nowMotionRatio
    {
        get { return easAnim.nowRatio; }
    }

    #region Existプロパティ
    public Action startAction
    {
        get { return exist.start; }
        set { exist.start = value; }
    }

    public Action enableAction
    {
        get { return exist.enable; }
        set { exist.enable = value; }
    }
    public Action endAction
    {
        get { return interval.activeAction; }
        set { interval.activeAction = value; }
    }
    #endregion

    #region Thresholdプロパティ
    public Action withinThreshold
    {
        get { return motionThreshold.withinRangeAction.action; }
        set { motionThreshold.withinRangeAction.action = value; }
    }

    public Action inThreshold
    {
        get { return motionThreshold.inRangeAction; }
        set { motionThreshold.inRangeAction = value; }
    }

    public Action exitThreshold
    {
        get { return motionThreshold.exitRangeAction.action; }
        set { motionThreshold.exitRangeAction.action = value; }
    }

    public Action outThreshold
    {
        get { return motionThreshold.outOfRangeAction; }
        set { motionThreshold.outOfRangeAction = value; }
    }
    #endregion

}
