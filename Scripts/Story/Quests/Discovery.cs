using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DiscoveryGroup : IEnumerable<Discovery> {

    private List<Discovery> _discoveries;

    public string label { get; set; }

    public DiscoveryGroup()
    {
        _discoveries = new List<Discovery>();
    }

    public void Add(Discovery discovery)
    {
        _discoveries.Add(discovery);
        discovery.group = this;
    }

    public IEnumerator<Discovery> GetEnumerator()
    {
        return _discoveries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _discoveries.GetEnumerator();
    }
}

public class Discovery
{
    private string _text;

    public string id { get; set; }

    public DiscoveryGroup group { get; set; }

    public string text
    {
        set { _text = value; }
    }

    internal string GetText(string targetName)
    {
        return _text.Replace("{t}", targetName);
    }
}
