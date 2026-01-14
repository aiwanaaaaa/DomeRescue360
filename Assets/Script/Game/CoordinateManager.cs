using UnityEngine;
using System.Collections.Generic;

public class CoordinateManager : MonoBehaviour
{
    public Transform player;
    public List<Transform> targets; // 監視したいオブジェクトたち

    // 構造体を使ってリストを作成
    public List<SphericalCoordinate> sphericalDataList = new List<SphericalCoordinate>();

    void Update()
    {
        sphericalDataList.Clear();

        foreach (var target in targets)
        {
            if (target == null) continue;

            // 計算
            Vector3 relativePos = target.position - player.position;
            float r = relativePos.magnitude;
            float a = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;
            float e = Mathf.Asin(relativePos.y / r) * Mathf.Rad2Deg;

            // 構造体を作成してリストに追加
            SphericalCoordinate data = new SphericalCoordinate(r, a, e);
            sphericalDataList.Add(data);
        }
    }
}