using System.Collections.Generic;
using UnityEngine;

public static class JsonArrayParsor
{
    public static List<string> Parse(string json)
    {
        // UnityのJsonUtilityは直接配列を扱えないため、ラップしてパース
        string wrapped = "{\"items\":" + json + "}";
        Wrapper wrapper = JsonUtility.FromJson<Wrapper>(wrapped);
        return wrapper.items;
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<string> items;
    }
}