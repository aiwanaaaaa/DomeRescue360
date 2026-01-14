using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ReticleController : MonoBehaviour
{
    [SerializeField] private SensorFetvher sensorFetcher;
    [SerializeField] private Transform playerCenter;

    [Header("Reticle Settings")]
    [SerializeField] private RawImage reticleImage;
    [SerializeField] private float reticleDistance = 5f;

    [Header("Detection Settings")]
    public List<SphericalTarget> targets;
    public float threshold = 10f; // 判定の広さ（度）

    void Update()
    {
        if (sensorFetcher == null || playerCenter == null) return;

        // 今回は最初のデバイスのデータを使用する例
        if (sensorFetcher.deviceIdList.Count > 0)
        {
            string id = sensorFetcher.deviceIdList[0];
            if (sensorFetcher.sensorMap.ContainsKey(id))
            {
                var sData = sensorFetcher.sensorMap[id];

                // 1. レティクルの位置を更新 (球面座標からワールド座標へ)
                UpdateReticlePosition(sData.alpha, sData.beta);

                // 2. 各ターゲットの座標更新と判定
                foreach (var target in targets)
                {
                    if (target == null) continue;

                    // ターゲットの現在の球面座標を更新
                    target.UpdateCoordinate(playerCenter.position);

                    // センサーの alpha(方位角), beta(仰角) と比較
                    if (IsMatching(target.currentCoords, sData.alpha, sData.beta))
                    {
                        Debug.Log($"{target.name} を捉えました！");
                        // ここにターゲットへの処理を記述
                    }
                }
            }
        }
    }

    // センサー角度を元にレティクルを配置
    void UpdateReticlePosition(float alpha, float beta)
    {
        if (reticleImage == null) return;

        // 球面座標から方向ベクトルを逆算
        // alpha = azimuth, beta = elevation
        float aRad = alpha * Mathf.Deg2Rad;
        float eRad = beta * Mathf.Deg2Rad;

        // 球面座標系 (r, θ, φ) から直交座標系 (x, y, z) への変換
        float x = Mathf.Sin(aRad) * Mathf.Cos(eRad);
        float y = Mathf.Sin(eRad);
        float z = Mathf.Cos(aRad) * Mathf.Cos(eRad);

        Vector3 lookDir = new Vector3(x, y, z);

        // レティクルの位置と向きを更新
        reticleImage.rectTransform.position = playerCenter.position + (lookDir * reticleDistance);
        reticleImage.rectTransform.rotation = Quaternion.LookRotation(lookDir);
    }

    bool IsMatching(SphericalCoordinate targetCoords, float alpha, float beta)
    {
        // 方位角(alpha)の差分を計算（360度跨ぎを考慮）
        float dAzimuth = Mathf.Abs(Mathf.DeltaAngle(alpha, targetCoords.azimuth));
        // 仰角(beta)の差分
        float dElevation = Mathf.Abs(beta - targetCoords.elevation);

        return dAzimuth < threshold && dElevation < threshold;
    }
}