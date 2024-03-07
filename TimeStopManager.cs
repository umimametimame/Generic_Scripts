using AddClass;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeStopManager : MonoBehaviour
{
    public static TimeStopManager instance;

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (TimeStopManager)FindObjectOfType(typeof(TimeStopManager));
            DontDestroyOnLoad(gameObject); // í«â¡
        }
        else Destroy(gameObject);

        SceneManager.sceneLoaded += LoadOtherScene;
    }

    [field: SerializeField] public float beforeTimeScale { get; private set; }
    [field: SerializeField] public bool stopping { get; private set; }
    public void TimeStop(float beforeScale)
    {
        if(stopping == false)
        {
            beforeTimeScale = beforeScale;
            stopping = true;
        }

        Time.timeScale = 0.0f;
    }

    public float TimeStart()
    {
        stopping = false;
        Time.timeScale = beforeTimeScale;
        return beforeTimeScale;
    }

    /// <summary>
    /// GameSceneà»äOÇì«Ç›çûÇÒÇæÇÁTimeScaleÇñﬂÇ∑
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void LoadOtherScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "GameScene")
        {
            Time.timeScale = 1.0f;
        }
    }
}
