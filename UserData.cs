using AddUnityClass;
using System;
using UnityEngine;

[Serializable]
public class UserData
{
    public string userID;
    public string userName;
    public void Assign(string _userID, string _userName)
    {
        userID = _userID;
        userName = _userName;
    }
}

[Serializable]
public class InRoomParameter
{
    [field: SerializeField, NonEditable] public InRoomEnum inRoomEnum { get; set; } = InRoomEnum.None;
    public SceneEnum currentScene 
    {
        get
        {
            return (SceneEnum)SceneList.GetSceneIndex();
        }
    } 
    

    
    public void Ready()
    {
        inRoomEnum = InRoomEnum.Ready;
    }

}

public enum InRoomEnum
{
    None,
    Ready,
    InBattle,
    Offline
}