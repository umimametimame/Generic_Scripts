using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentUser : MonoBehaviour
{
    public static CurrentUser instance;
    public Rakuin_CurrentlyUserData userInfo { get; private set; }

    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        userInfo = GetComponent<Rakuin_CurrentlyUserData>();
    }

    private void Singleton()
    {
        if (instance == null)
        {
            instance = (CurrentUser)FindAnyObjectByType(typeof(CurrentUser));
            DontDestroyOnLoad(gameObject); // ’Ç‰Á
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
