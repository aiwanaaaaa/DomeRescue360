using UnityEngine;
using System.Collections.Generic;

public class SensorRaycaster : MonoBehaviour
{
    [SerializeField] private GameObject sensor;
    private SensorFetvher data;

    public List<SphericalTarget> targetList; // SphericalTargetがついたオブジェクトのリスト
    public float threshold = 10f; // 判定の甘さ（度数）

    void Update()
    {
        if (data == null) data = sensor.GetComponent<SensorFetvher>();
        if (data == null) return;

        foreach (string id in data.deviceIdList)
        {
            if (data.sensorMap.ContainsKey(id))
            {
                var sensor = data.sensorMap[id];
                float alpha = sensor.alpha;
                float beta = sensor.beta;
                // 1. 各ターゲットの球面座標を更新（プレイヤーとの相対位置から）
                // 2. センサー角度とターゲットの保持している角度を比較
                foreach (var target in targetList)
                {
                    //target.UpdateCoordinates(this.transform);

                    if (IsMatching(target.currentCoords,sensor.alpha,sensor.beta))
                    {
                        Debug.Log($"{target.name} がレティクル内に入りました！");
                        // ここに対象物への処理（色を変える、音を鳴らす等）を書く
                    }
                }
            }
        }
    }

    bool IsMatching(SphericalCoordinate targetCoords,float alpha,float beta)
    {

        // センサーの alpha (方位角), beta (仰角) と比較
        float dAzimuth = Mathf.Abs(Mathf.DeltaAngle(alpha, targetCoords.azimuth));
        float dElevation = Mathf.Abs(beta - targetCoords.elevation);

        return dAzimuth < threshold && dElevation < threshold;
    }
}