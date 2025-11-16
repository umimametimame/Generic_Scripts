using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SrtingEnum", menuName = "Scriptable Objects/SrtingEnum")]
public class StringEnum_Profile : ScriptableObject
{
    [field: SerializeField] public List<string> enums { get; private set; } = new List<string>();

    public StringEnum Convert
    {
        get
        {
            StringEnum _newEnum = new StringEnum(enums);
            return _newEnum;
        }
    }
}
