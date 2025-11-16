using AddUnityClass;
using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// MoveActionをイベントとして持つ
/// </summary>
public class InputOperator : NetworkBehaviour, INetworkRunnerCallbacks
{
    [field: SerializeField] public PlayerInput playerInput { get; private set; }
    [field: SerializeField] public List<InputVecOrFloat<Vector3>> vInputs = new List<InputVecOrFloat<Vector3>>();
    [field: SerializeField] public List<InputVecOrFloat<float>> fInputs = new List<InputVecOrFloat<float>>();
    [field: SerializeField, NonEditable] public InputVecOrFloat<Vector3> moveInput { get; private set; } = new InputVecOrFloat<Vector3>();
    public Vector3 beforeMoveInput { get; set; }
    [field: SerializeField, NonEditable] public InputVecOrFloat<Vector3> viewPointInput = new InputVecOrFloat<Vector3>();
    [SerializeField] protected Camera viewPointCamera;
    public void Initialize()
    {
        playerInput = GetComponent<PlayerInput>();
        moveInput.Initialize();
        foreach(var i in vInputs)
        {
            i.Initialize();
        }
        foreach (var i in fInputs)
        {
            i.Initialize();
        }

    }
    public override void FixedUpdateNetwork()
    {
        beforeMoveInput = moveInput.entity;
    }

    public void SetInputsList()
    {

        TypeFinder t = gameObject.AddComponent<TypeFinder>();
        vInputs = t.GetAndInList<InputVecOrFloat<Vector3>>(GetType());

        fInputs = t.GetAndInList<InputVecOrFloat<float>>(GetType());
        Destroy(t);
    }

    /// <summary>
    /// カメラの向いている方向に移動方向を正規化した値
    /// </summary>
    public Vector3 GetCameraNormalizedMoveVelocity()
    {
        Vector3 nor = Vector3.zero;

        Transform camTra = viewPointCamera.transform;
        Quaternion cameraRotation = Quaternion.Euler(0f, camTra.rotation.eulerAngles.y, 0f);
        // 高さを除いたベクトルに変換
        //nor = (camTra.right * moveInput.entity.x) + (camTra.forward * moveInput.entity.z);
        //nor.y = 0;

        nor = cameraRotation * moveInput.entity;
        nor.Normalize();
        return nor;
    }

    /// <summary>
    /// 引数の方向に移動方向を正規化した値
    /// </summary>
    /// <param name="_transformRef"></param>
    /// <returns></returns>
    public Vector3 GetNormalizedMoveVelocity(Vector3 _right, Vector3 _forward)
    {

        Vector3 nor = Vector3.zero;
        // 高さを除いたベクトルに変換
        nor = (_right * moveInput.entity.x) + (_forward * moveInput.entity.z);
        nor.y = 0;
        nor.Normalize();
        return nor;
    }

    public bool IsMoving
    {
        get
        {
            if (moveInput.entity != Vector3.zero)
            {

                if (beforeMoveInput != Vector3.zero)
                {
                    return true;
                }
            }
            return false;
        }
    }


    #region PlayerInputEvent
    public void OnMove(InputValue value)
    {
        //if (playerInput.currentActionMap.controlSchemes.ToString() != "KeyBoard")
        //{
        //    return;
        //}
        //Vector3 newVec = Vector3.zero;
        //newVec.x = value.Get<Vector2>().x;
        //newVec.z = value.Get<Vector2>().y;

        //moveInput.entity = newVec;

    }

    public void AssignMoveStick()
    {
        if(playerInput != null)
        {
            Gamepad _gamepad = Gamepad.current;
            Vector3 _newVec = Vector3.zero;
            _newVec.x = _gamepad.leftStick.ReadValue().x;
            _newVec.z = _gamepad.leftStick.ReadValue().y;

            moveInput.entity = _newVec;
            Debug.Log($"{transform.root.name} + {moveInput.entity}");
        }
    }

    /// <summary>
    /// 自前のTPSViewPointではxとyが反転する
    /// </summary>
    /// <param name="value"></param>
    public void OnMoveViewPoint(InputValue value)
    {
        //Vector3 newVec = Vector3.zero;
        //newVec.x = -value.Get<Vector2>().y;
        //newVec.y = value.Get<Vector2>().x;

        //viewPointInput.entity = newVec;
    }

    public void AssignMoveViewPointStick()
    {
        if (playerInput != null)
        {
            Gamepad _gamepad = Gamepad.current;
            Vector3 _newVec = Vector3.zero;
            _newVec.x = _gamepad.rightStick.ReadValue().y * -1;
            _newVec.y = _gamepad.rightStick.ReadValue().x;

            viewPointInput.entity = _newVec;
        }
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

    public virtual void OnInput(NetworkRunner runner, Fusion.NetworkInput input)
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
    #endregion

}


[Serializable]
public class Inputting
{
    public enum InputType
    {
        None,
        Vector3,
        Float,
    }
    public virtual void Initialize() { }
}
/// <summary>
/// TはVector3またはfloat<br/>
/// EntityAndPlanとboolを含む
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable] public class InputVecOrFloat<T> : Inputting where T : struct
{
    [field: SerializeField, NonEditable] public EntityAndPlan<T> input { get; set; } = new EntityAndPlan<T>();
    [field: SerializeField] public ValueInRange floatRange { get; private set; } = new ValueInRange();
    [SerializeField, NonEditable] private InputType thisType;
    public bool inputting
    {
        get
        {
            switch (thisType)
            {
                case InputType.Vector3:
                    if ((Vector3)(object)input.entity != Vector3.zero)
                    {
                        return true;
                    }
                    break;

                case InputType.Float:
                    if ((float)(object)input.entity != 0.0f)
                    {
                        if (floatRange.min == 0.0f && floatRange.max == 0.0f)
                        {
                            return true;
                        }
                        else
                        {
                            return floatRange.IsInRange((float)(object)input.entity);
                        }
                    }
                    break;

                default:
                    break;
            }
            return false;
        }
    }
    public override void Initialize()
    {
        if (typeof(T).Equals(typeof(Vector3)))
        {
            thisType = InputType.Vector3;
        }
        else if (typeof(T).Equals(typeof(float)))
        {
            thisType = InputType.Float;
        }

    }


    public void Assign()
    {
        input.Assign();
    }

    public T entity
    {
        get { return input.entity; }
        set { input.entity = value; }
    }
    public T plan
    {
        get { return input.plan; }
        set { input.plan = value; }
    }
    
}