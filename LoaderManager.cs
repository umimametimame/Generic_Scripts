using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderManager : MonoBehaviour
{
    [SerializeField] private List<Loader> loaders = new List<Loader>();
    [field: SerializeField] public bool finished { get; private set; }
    [field: SerializeField, NonEditable] public float persent { get; private set; }

    private void Start()
    {
        FindAllLoaderComponent();
        LoadStart();
    }

    private void Update()
    {
        CheckFinished();
    }


    /// <summary>
    /// ヒエラルキー上のLoaderコンポーネントを全て取得
    /// </summary>
    private void FindAllLoaderComponent()
    {
        GameObject[] targets = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject target in targets)
        {
            Loader targetLoader = target.GetComponent<Loader>();
            if (targetLoader != null)
            {
                loaders.Add(targetLoader);
            }

        }
    }

    private void LoadStart()
    {
        foreach (Loader loader in loaders)
        {
            loader.StartLoad();
        }
    }

    private void CheckFinished()
    {
        finished = true;
        int sumComponent = loaders.Count;
        int finishedComponent = 0;
        foreach(Loader loader in loaders)
        {
            if(loader.finishLoad == true)
            {
                finishedComponent++;
            }
            else
            {
                finished = false;
            }
        }

        persent = finishedComponent / sumComponent;
    }
}
