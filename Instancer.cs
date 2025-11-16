using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自動で削除されるObjectにのみ使用
/// </summary>
[Serializable] public class Instancer
{
    public enum DisplayState
    {
        NotDisplayYet,
        Displaying,
        Death,
    }
    [field: SerializeField] public GameObject obj { get; set; }
    public bool isPhotonNetworkObject;
    [field: SerializeField, NonEditable] public List<GameObject> clones { get; set; } = new List<GameObject>();
    [field: SerializeField, NonEditable] public DisplayState state { get; private set;}
    [SerializeField] private List<AudioClip> randomInstanceSound = new List<AudioClip>();
    [SerializeField] public GameObject parent;

    public Instancer()
    {

        //PhotonNetwork.PrefabPool = this;
    }

    /// <summary>
    /// 改良中につき使用しない事<br/>
    /// Initialize時に指定した場合、親子関係が作られる
    /// </summary>
    [field: SerializeField] public Instancer afterObj;

    /// <summary>
    /// 引数2に親オブジェクトを指定することが出来る。
    /// </summary>
    /// <param name="_afterObj"></param>
    /// <param name="_parent"></param>
    public virtual void Assign(Instancer _afterObj = null, GameObject _parent = null)
    {
        if(_parent != null) 
        { 
            parent = _parent; 
        }

        state = DisplayState.NotDisplayYet;

        if (_afterObj != null) 
        {
            afterObj = _afterObj; 
        }
    }


    /// <summary>
    /// Listの更新
    /// </summary>
    public virtual void Update()
    {

        for(int i = clones.Count - 1; i >= 0; i--)
        {
            if (clones[i] != null) { state = DisplayState.Displaying; } // 一つでも表示中なら
            else { clones.RemoveAt(i); }

        }

        switch (state)
        {
            case DisplayState.NotDisplayYet:
                break;

            case DisplayState.Displaying:
                if (clones.Count == 0) { state = DisplayState.Death; }
                break;

            case DisplayState.Death:
                break;
        }
    }
    public virtual void Instance()
    {
        InstanceSound();

        GameObject _clone;
        if (obj != null)
        {
            if (isPhotonNetworkObject == true)
            {
                
                if (parent != null)
                {
                    _clone = GameObject.Instantiate(obj, parent.transform);
                }
                else
                {

                    _clone = GameObject.Instantiate(obj);
                }
            }
            else
            {

                if (parent != null)
                {
                    _clone = GameObject.Instantiate(obj, parent.transform);
                }
                else
                {

                    _clone = GameObject.Instantiate(obj);
                }
            }
            clones.Add(_clone);

        }

    }
    public virtual void Instance(GameObject _parent)
    {
        InstanceSound();
        if (obj != null)
        {
            GameObject _clone;

            if (isPhotonNetworkObject == true)
            {
                _clone = GameObject.Instantiate(obj, _parent.transform);
            }
            else
            {
                _clone = GameObject.Instantiate(obj, _parent.transform);
            }

            clones.Add(_clone);
        }


    }
    public virtual GameObject Instance(Transform _instancePos)
    {
        InstanceSound();
        if (obj != null)
        {
            GameObject _clone = GameObject.Instantiate(obj);
            if (isPhotonNetworkObject == true)
            {

                _clone.transform.position = _instancePos.position;
            }
            else
            {
                _clone.transform.position = _instancePos.position;
            }
            clones.Add(_clone);

            return _clone;
        }

        return null;
    }

    public NetworkObject Spawn(NetworkRunner _runner, PlayerRef _player, Vector3? _position = null, Quaternion? _rotation = null)
    {
        NetworkObject _instanced = _runner.Spawn(obj, _position, _rotation,_player);
        clones.Add(_instanced.gameObject);
        return _instanced;
    }

    private void InstanceSound()
    {

        if (isHaveSound == true)
        {
            FrontCanvas.instance.source.PlayOneShot(randomSound);
        }
    }

    /// <summary>
    /// 引数がtrueの場合、出現させていないなら出現させる
    /// </summary>
    public void SetActiveClones(bool _active)
    {

        if (instanced == false)
        {
            Instance();
        }

        for (int i = 0; i < clones.Count; i++)
        {
            clones[i].SetActive(_active);
        }
    }



    /// <summary>
    /// stateがNotDisplayYetの場合のみInstanceを行う
    /// </summary>
    /// <param name="parent"></param>
    public virtual void InstanceOnlyOnce(GameObject parent = null)
    {
        if (state == DisplayState.NotDisplayYet)
        {
            if (parent == null) { Instance(); }
            else { Instance(parent); }
        }
    }

    /// <summary>
    /// clonesの最後を渡す
    /// </summary>
    public GameObject lastObj
    {
        get { return clones[clones.Count - 1]; }
        private set { clones[clones.Count -1] = value; }
    }

    public List<T> GetClonecomponents<T>()
    {
        List<T> _components = new List<T>();
        for(int i = 0; i <  clones.Count; i++)
        {
            _components.Add(clones[i].GetComponentInChildren<T>());
        }

        return _components;
    }

    public void DestroyClones()
    {
        for (int i = 0; i < clones.Count; i++)
        {
            GameObject.Destroy(clones[i].gameObject);
        }

        clones = new List<GameObject>();
    }

    public bool displaying
    {
        get
        {
            switch (state)
            {
                case DisplayState.NotDisplayYet:
                    return false;

                case DisplayState.Displaying:
                    return true;

                case DisplayState.Death:
                    return false;
            }

            return false;
        }
    }

    /// <summary>
    /// 生成済みかどうか
    /// </summary>
    public bool instanced
    {
        get
        {
            if(clones.Count > 0) // 出現済みなら
            {
                return true;
            }
            return false;
        }
    }
    public AudioClip randomSound
    {
        get
        {
            int _random = UnityEngine.Random.Range(0, randomInstanceSound.Count - 1);
            return randomInstanceSound[_random];
        }
    }

    public bool isHaveSound
    {
        get
        {
            if(randomInstanceSound.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
