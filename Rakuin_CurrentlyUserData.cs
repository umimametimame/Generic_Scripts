using AddUnityClass;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rakuin_CurrentlyUserData : SingletonDontDestroy<Rakuin_CurrentlyUserData>
{
    [field: SerializeField] public UserData userData { get; private set; } = new UserData();
    [field: SerializeField] private Instancer inputField = new Instancer();
    private void Start()
    {
        userData = new UserData();
        if (JsonOperator.ExistJson(JsonFileName.UserData) == false)
        {
            inputField.Instance();
        }
        else
        {
            userData = JsonOperator.Read<UserData>(JsonFileName.UserData);
        }
        
    }

    private void Update()
    {
        inputField.Update();
    }

    public void Assign(string _userID, string _userName)
    {
        userData.Assign(_userID, _userName);
    }

    public void Event_InitialRegistration(string _userName)
    {
        string _registDate = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();
        Assign(_registDate, _userName);

        JsonOperator.Write(userData, JsonFileName.UserData);
    }


}
