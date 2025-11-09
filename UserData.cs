using System;

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
