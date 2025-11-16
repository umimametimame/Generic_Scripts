using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Multiplayer.Playmode;
using System.Linq;

public class Fusion_Connect : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Instancer prefab;
    [SerializeField, NonEditable] private NetworkRunner instancedNetworkRunner;
    public static string networkRunnerTag = "NetworkRunner";
    public static Fusion_Connect instance;
    public static NetworkRunner networkRunner
    {
        get
        {
            return instance.instancedNetworkRunner;
        }
    }

    private void Singleton()
    {
        if (instance == null)
        {
            instance = GameObject.FindAnyObjectByType<Fusion_Connect>();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Singleton();
        TagOperator.AddTag(networkRunnerTag);
        prefab.Instance(gameObject);
        prefab.lastObj.tag = networkRunnerTag;
        instancedNetworkRunner = prefab.lastObj.GetComponent<NetworkRunner>();
    }
    private void Start()
    {
        JoinRoom();

    }
    
    private async void JoinRoom()
    {
        if(CurrentPlayer.ReadOnlyTags().Contains("Main") == false)
        {
            Debug.Log("ì¸é∫ë“ã@");
            await Task.Delay(5000);
        }
        StartGameArgs _args = new StartGameArgs();
        _args.GameMode = GameMode.AutoHostOrClient;
        _args.SceneManager = instancedNetworkRunner.GetComponent<NetworkSceneManagerDefault>();

        StartGameResult _result = await instancedNetworkRunner.StartGame(_args);

        if (_result.Ok)
        {
            Debug.Log("ê⁄ë±");
        }
        else
        {
            Debug.Log("é∏îs");
        }
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, Fusion.NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
    {
        
    }

    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
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

    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        
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

    void INetworkRunnerCallbacks.OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    void INetworkRunnerCallbacks.OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }
}
