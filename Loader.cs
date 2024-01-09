using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funcの登録またはFinishLoadを実行してLoadを完了させる
/// </summary>
public class Loader : MonoBehaviour
{
    [SerializeField] private bool startLoad;
    [field: SerializeField] public bool finishLoad { get; private set; }
    public Func<bool> loadFunc { get; set; }    // ロード終了時にtrueを返す関数
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
