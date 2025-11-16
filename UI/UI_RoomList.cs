using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoomList : UI_ListContent, INetworkRunnerCallbacks
{
    public List<GameObject> notFoundPanels;
    public Rakuin_ListContent_RoomInfo roomInfoObj;
    public UI_Button joinButton;
    private bool pressableJoinButton;
    public UI_Button reloadButton;
    [field: SerializeField, NonEditable] public List<SessionInfo> roomInfoRef { get; private set; } = new List<SessionInfo> ();
    [field: SerializeField] public List<Rakuin_RoomInfo> roomInfo { get; private set; } = new List<Rakuin_RoomInfo>();
    private void Awake()
    {
        isChangeTextToContentTitle = true;
        pressableJoinButton = false;

        SetActiveSample(false);
    }
    private void Start()
    {
        joinButton.IsPressableCondition = true;
        joinButton.onClickAction += roomInfoObj.Event_JoinRoom;
        reloadButton.onClickAction += Event_GetRoomList;
    }

    private void FixedUpdate()
    {
        if (joinButton != null)
        {
            joinButton.AssignPressable(pressableJoinButton);
        }
    }

    public void Event_GetRoomList()
    {

        pressableJoinButton = false;
        instancer.DestroyClones();
        roomInfoObj.gameObject.SetActive(false);
        roomInfo = new List<Rakuin_RoomInfo>();
        List<string> _roomNameList = new List<string>();

        for(int i = 0; i < roomInfoRef.Count; i++)
        {
            roomInfo.Add(new Rakuin_RoomInfo());
            roomInfo[i].Assign(roomInfoRef[i].Name);
            _roomNameList.Add(roomInfo[i].roomName);
        }

        if (roomInfo.Count > 0)
        {
            SetActiveNotFoundUI(false);
            Assign_ContentTitles(_roomNameList);
            InstanceList();
            SetActiveClones(true);
            var _roomInfoComponents = AddComponentsInInstancedList<Rakuin_RoomInfo>();
            var _event = GetComponentsInInstancedList<RegisterEventTrigger>();

            for (int i = 0; i < _roomInfoComponents.Count; i++)
            {
                _roomInfoComponents[i] = roomInfo[i];

                int _index = i;
                _event[i].onClick += () => Event_RoomSelect(roomInfo[_index]);
            }
        }
        else if (roomInfo.Count <= 0)
        {
            SetActiveNotFoundUI(true);
        }
    }

    private void SetActiveNotFoundUI(bool _active)
    {
        for (int i = 0; i < notFoundPanels.Count; i++)
        {
            notFoundPanels[i].SetActive(_active);
        }
    }

    private void Event_RoomSelect(Rakuin_RoomInfo _roomInfo)
    {
        roomInfoObj.gameObject.SetActive(true);
        roomInfoObj.Event_AssignRoomInfo(_roomInfo);
        pressableJoinButton = true;
    }


    private void OnEnable()
    {
        Event_GetRoomList();
    }

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, Fusion.NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        
    }

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        roomInfoRef.Clear();
        roomInfoRef = sessionList;

    }

    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner)
    {
        
    }


}
