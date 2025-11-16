using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomOperator : MonoBehaviour
{
    [field: SerializeField, NonEditable] public List<Fusion_UserData> players { get; private set; } = new List<Fusion_UserData>();
    private List<ValueChecker<InRoomEnum>> playersReadyChecker = new List<ValueChecker<InRoomEnum>>();
    private ValueChecker<int> numberOfPlayersChecker = new ValueChecker<int>();
    [field: SerializeField] public Interval transitionIntervalByReady { get; set; } = new Interval();
    private void Awake()
    {


        numberOfPlayersChecker.Initialize(ChildCount);
        numberOfPlayersChecker.changedAction += AssignPlayers;

        transitionIntervalByReady.Initialize(false, false);
    }

    private void Update()
    {
        numberOfPlayersChecker.Update(ChildCount);
        for (int i = 0; i < playersReadyChecker.Count; i++)
        {
            playersReadyChecker[i].Update(players[i].inRoomRef.inRoomEnum);
        }
    }

    public void AssignPlayers()
    {
        players = GetComponentsInChildren<Fusion_UserData>().ToList();
        playersReadyChecker.Clear();
        for (int i = 0; i < players.Count; i++)
        {
            playersReadyChecker.Add(new ValueChecker<InRoomEnum>());
            playersReadyChecker[i].Initialize(players[i].inRoomRef.inRoomEnum);
            playersReadyChecker[i].changedAction += OnChangeInRoomEnum;
        }

        OnChangeInRoomEnum();
    }


    public void Assign_Rule()
    {

    }

    /// <summary>
    /// 全プレイヤー毎のEnumが変わったイベント
    /// </summary>
    public void OnChangeInRoomEnum()
    {
        if (IsAllPlayerReady == true)
        {
            Rakuin_SceneManager.instance.LaunchSceneLoad(SceneEnum.Battle);
        }
        else
        {

        }
    }

    /// <summary>
    /// すべてのプレイヤーが準備完了を押しているか
    /// </summary>
    public bool IsAllPlayerReady
    {
        get
        {
            if (players.Count < 2)
            {
                return false;
            }

            bool _result = true;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].inRoomRef.inRoomEnum != InRoomEnum.Ready)
                {
                    _result = false;
                }
            }

            return _result;
        }
    }

    //public bool IsAllPlayerSceneLoaded
    //{
    //    get
    //    {
    //        for (int i = 0; i < players.Count; i++)
    //        {
    //            if (players[i].inRoomRef.currentScene
    //        }
    //    }
    //}

    public int ChildCount
    {
        get
        {
            return transform.childCount;
        }
    }
}
