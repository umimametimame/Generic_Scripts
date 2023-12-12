using System.Collections.Generic;
using UnityEngine;
using AddClass;
using GenericChara;
using System;

public class MotionCollider : MonoBehaviour
{
    [field: SerializeField] public Chara parent { get; set; }
    [field: SerializeField, NonEditable] public bool enable { get; private set; }
    [SerializeField] private Collider thisCollider;
    [field: SerializeField, NonEditable] public int hitCount { get; private set; }
    [SerializeField, NonEditable] private float damage;
    [SerializeField] private List<int> hitCountEntitys = new List<int>();
    [SerializeField] private List<Chara> targets = new List<Chara>();

    public Func<bool, Collider, bool> passJudgeFunc;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (thisCollider == null) { thisCollider = GetComponent<Collider>(); }
        Reset();
    }
    public void Reset()
    {
        enable = false;
        hitCount = 0;
        damage = 0.0f;
        hitCountEntitys.Clear();
        targets.Clear();
    }

    /// <summary>
    /// ����:<br/>
    /// �E�_���[�W<br/>
    /// �E�q�b�g��
    /// </summary>
    public void Launch(float damage, int hitCount = 1)
    {
        this.damage = damage;
        this.hitCount = hitCount;
        enable = true;
    }

    private void OnTriggerStay(Collider you)
    {
        bool passing = true;
        if(enable == false) { passing = false; }
        passing = passJudgeFunc.Invoke(passing, you);
        //if(!(you.tag == Tags.Player01 || you.tag == Tags.Player02)) { passing = false; }
        if (passing == false) { return; }
        bool firstTime = false;
        bool attacked = false;
        if(targets.Count == 0 ) 
        {
            firstTime = true;
        }
        else
        {
            foreach (Chara c in targets)  // targets�����[�v����
            {
                if (c == you)
                {
                    firstTime = false;
                }
            }

        }

        if (firstTime == true)          // ����̂łȂ����
        {                               // targets�ɒǉ�����
            targets.Add(you.transform.root.GetChild(0).GetComponent<Chara>());
            hitCountEntitys.Add(0);
        }

        for(int i = 0; i < targets.Count; ++i)
        {
            if (hitCountEntitys[i] != hitCount)
            {
                attacked = targets[i].UnderAttack(damage, UnderAttackType.Normal, parent);    // �U���o������
                if(attacked == true) { 
                    if(hitCount >= 1)
                    {
                        hitCountEntitys[i]++;   // �q�b�g������
                    }      
                }
            }
        }


    }
}

public enum UnderAttackType
{
    None,
    Normal,
}