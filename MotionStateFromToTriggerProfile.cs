using System;
using UnityEngine;

public class MotionStateFromToTriggerProfile<T> : ScriptableObject where T : Enum
{
    public T from;
    public T to;
    public string triggerValue;

    public bool IsMatch(T _from, T _to)
    {
        bool _isBothMatch = false;
        if (from.Equals(_from) == true)
        {
            if (to.Equals(_to) == true)
            {
                _isBothMatch = true;
            }
        }

        return _isBothMatch;
    }
}
