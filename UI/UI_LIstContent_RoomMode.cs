using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LIstContent_RoomMode : UI_ListContent
{
    private SerializedDictionary<RoomJoinType, GameObject> instancedList = new SerializedDictionary<RoomJoinType, GameObject>();
    public GameObject roomCreateUI;
    public GameObject roomSerchUI;

    private void Awake()
    {
        isChangeTextToContentTitle = true;
        List<RoomJoinType> _enums = new List<RoomJoinType>();
        _enums = EnumOperator.Get<RoomJoinType>();
        Assign_ContentTitles(_enums);
        InstanceList();

        instancedList = GetInstancedListWithEnum<RoomJoinType>();
        
        foreach (var _obj in instancedList)
        {
            var _event = _obj.Value.GetComponentInChildren<RegisterEventTrigger>();

            switch (_obj.Key)
            {
                case RoomJoinType.Create:
                    _event.onClick += OnClickCreate;
                    break;
                case RoomJoinType.Serch:
                    _event.onClick += OnClickSerch;
                    break;
            }
        }
    }


    public void OnClickCreate()
    {
        roomCreateUI.SetActive(true);
    }

    public void OnClickSerch()
    {
        MenuSceneOperator.instance.uniqueRoomMenu.Switch(MenuCanvas.Room_Serch);
    }
}


/// <summary>
/// ルームモードでの参加方法
/// </summary>
public enum RoomJoinType
{
    Create,
    Serch,
}
