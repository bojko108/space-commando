using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFeatureAttributes
{
    private Dictionary<string, string> attributes;

    public MapFeatureAttributes(Dictionary<string, string> attributes)
    {
        this.attributes = attributes;
    }

    public void Add(Dictionary<string,string> attributes)
    {
        foreach(KeyValuePair<string,string> kv in attributes)
        {
            this.attributes.Add(kv.Key, kv.Value);
        }
    }

    public T Get<T>(string attributeName)
    {
        string value;
        this.attributes.TryGetValue(attributeName, out value);
        return (T)Convert.ChangeType(value, typeof(T));
    }
}
