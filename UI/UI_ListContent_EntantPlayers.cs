using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class UI_ListContent_EntantPlayers : UI_ListContent
{
    private List<PlayerRef> playerList = new List<PlayerRef>();
    private void Awake()
    {
        
    }

    public void GetEntantPlayer()
    {
        playerList = new List<PlayerRef>();
        List<string> _names = new List<string>();
        
        
        playerList.AddRange(Fusion_Connect.networkRunner.ActivePlayers);

        for (int i = 0; i < playerList.Count; i++)
        {
            _names.Add(playerList[i].PlayerId.ToString());
        }


        instancer.DestroyClones();
        isChangeTextToContentTitle = true;
        Assign_ContentTitles(_names);
        InstanceList();
        SetActiveClones(true);
    }

    public void Event_UpdatePlayer(PlayerRef _player)
    {
        GetEntantPlayer();
    }

    public void Event_JoinPlayer(PlayerRef _newPlayer)
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].PlayerId == _newPlayer.PlayerId)
            {

            }
        }
    }


    public void Event_LeftPlayer(PlayerRef _otherPlayer)
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }


    }

}