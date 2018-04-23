using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[Serializable]
public struct LocalizedString
{
    [SerializeField]
    private SystemLanguage _language;

    [SerializeField]
    private string _value;

    public SystemLanguage language { get { return _language; } }

    public string value { get { return _value; } }
}

[Serializable]
public struct Response
{
    [SerializeField]
    private TextResource _answer;

    [SerializeField]
    private Discovery[] _conditions;

    [SerializeField]
    private Discovery[] unlocks;

    [SerializeField]
    private int _priority;

    public bool isNoteworthy
    {
        get { return unlocks.Length > 0; }
    }

    public string text
    {
        get { return _answer.value; }
    }
}
