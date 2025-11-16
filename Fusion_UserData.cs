using UnityEngine;

public class Fusion_UserData : MonoBehaviour
{
    [field: SerializeField, NonEditable] public UserData userDataRef { get; private set; } = new UserData();
    [field: SerializeField] public InRoomParameter inRoomRef { get; private set; } = new InRoomParameter();

    private void Awake()
    {
        //transform.parent = GameObject.FindWithTag(Connect_PUN.addedTag).transform;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }

    public void Assign(UserData _userData, InRoomParameter _inRoomRef)
    {
        userDataRef = _userData;
        inRoomRef = _inRoomRef;
        AssignObjName();
    }

    public void AssignObjName()
    {

        gameObject.name = userDataRef.userName;
    }
}

