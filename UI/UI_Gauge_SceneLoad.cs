using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Gauge_SceneLoad : MonoBehaviour
{
    private Slider gauge;
    private void Start()
    {
        gauge = GetComponent<Slider>();
    }

    private void Update()
    {
        gauge.value = Rakuin_SceneManager.instance.loadingScenePer;
    }
}
