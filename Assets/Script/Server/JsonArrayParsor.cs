using System.Collections.Generic;
using UnityEngine;

public static class JsonArrayParsor
{
    public static List<string> Parse(string json)
    {
        // Unity��JsonUtility�͒��ڔz��������Ȃ����߁A���b�v���ăp�[�X
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