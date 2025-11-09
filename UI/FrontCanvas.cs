using AddUnityClass;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioListener))]
public class FrontCanvas : SingletonDontDestroy<FrontCanvas>
{
    [field: SerializeField] public bool debugMode { get; private set; }
    [SerializeField] private SceneOperator sceneOperator;
    [field: SerializeField] public AudioSource source { get; private set; }
    protected virtual void Start()
    {
        source = GetComponent<AudioSource>();
        FindSceneOperator();
        SceneManager.sceneLoaded += SceneChanged;
    }
    protected virtual void Update()
    {
        //debugMode = sceneOperator.debugMode;
    }

    private void FindSceneOperator()
    {
        sceneOperator = GameObject.FindWithTag(Tags.SceneOperator).GetComponent<SceneOperator>();
    }

    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        FindSceneOperator();

    }
}