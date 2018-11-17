using System;
using UnityEngine;

public class LuaScript : ScriptableObject
{
    [SerializeField]
    [HideInInspector]
    string _text;
    public string text
    {
        get
        {
            return _text;
        }
    }

    public static LuaScript CreateFromString(string str)
    {
        var script = ScriptableObject.CreateInstance<LuaScript>();
        script._text = str;
        return script;
    }
}