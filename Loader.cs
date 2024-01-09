using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Func�̓o�^�܂���FinishLoad�����s����Load������������
/// </summary>
public class Loader : MonoBehaviour
{
    [SerializeField] private bool startLoad;
    [field: SerializeField] public bool finishLoad { get; private set; }
    public Func<bool> loadFunc { get; set; }    // ���[�h�I������true��Ԃ��֐�
    private void Start()
    {
        
    }
    private void Update()
    {
        if (startLoad == true && finishLoad == false)
        {
            if(loadFunc != null)
            {
                if(loadFunc() == true)
                {
                    FinishLoad();
                }
            }
        }
    }

    public void StartLoad()
    {
        startLoad = true;
    }
    public void FinishLoad()
    {
        finishLoad = true;
    }
}
