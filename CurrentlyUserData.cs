using System;
using UnityEngine;

public class CurrentlyUserData : MonoBehaviour
{
    [field: SerializeField] public UserData userData { get; private set; } = new UserData();
    [field: SerializeField] public InRoomParameter inRoomParam { get; private set; } = new InRoomParameter();
    [field: SerializeField] private Instancer inputField = new Instancer();
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        
    }


    private void Update()
    {
        inputField.Update();
    }
    public void Initialize()
    {
        userData = new UserData();
        if (JsonOperator.ExistJson(JsonFileName.UserData) == false)
        {
            Event_InstanceInputField();
        }
        else
        {
            userData = JsonOperator.Read<UserData>(JsonFileName.UserData);
            //PhotonNetwork.NickName = userData.userName;
            AssignName();
        }
    }
    public void Event_InstanceInputField()
    {
        inputField.Instance();
        UI_InputField _input = inputField.lastObj.GetComponent<UI_InputField>();
        _input.Assign(Event_InitialRegistration);
    }

    public void Assign(string _userID, string _userName)
    {
        userData.Assign(_userID, _userName);
    }

    public void Event_InitialRegistration(string _userName)
    {
        string _registDate = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond.ToString();
        Assign(_registDate, _userName);
        AssignName();

        JsonOperator.Write(userData, JsonFileName.UserData);
    }

    public void AssignName()
    {
        //PhotonNetwork.NickName = userData.userName;
    }

    public void AssignObjectName()
    {
        gameObject.name = userData.userName;
    }
}
