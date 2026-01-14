using UnityEngine;
using System;

public class CartesianToSphericalConverter : MonoBehaviour
{
    // 座標の原点となるTransform (nullの場合はワールド原点 (0, 0, 0) を使用)
    public Transform centerPoint;

    // 球面座標の結果を格納する構造体
    [Serializable]
    public struct SphericalCoordinates
    {
        public float radius; // r (距離)
        public float azimuthDegrees; // θ (方位角、度)
        public float polarAngleDegrees; // φ (天頂角、度)

        public override string ToString()
        {
            return $"R: {radius:F2}, Azimuth: {azimuthDegrees:F2}°, Polar: {polarAngleDegrees:F2}°";
        }
    }

    [Header("Current Spherical Coordinates")]
    public SphericalCoordinates result;

    void Update()
    {
        // ワールド座標を取得
        Vector3 worldPosition = transform.position;

        // 基準点からの相対座標を計算
        Vector3 relativePosition;
        if (centerPoint != null)
        {
            relativePosition = worldPosition - centerPoint.position;
        }
        else
        {
            relativePosition = worldPosition;
        }

        result = ConvertToSpherical(relativePosition);

        // 実行時にログに出力して確認
        Debug.Log($"Object: {gameObject.name} | World Pos: {worldPosition} | Spherical: {result}");
    }

    /// <summary>
    /// デカルト座標 (x, y, z) を球面座標 (r, θ, φ) に変換します。
    /// </summary>
    /// <param name="cartesian">原点からの相対的なデカルト座標</param>
    /// <returns>球面座標 (r, Azimuth, Polar)</returns>
    public static SphericalCoordinates ConvertToSpherical(Vector3 cartesian)
    {
        SphericalCoordinates sc = new SphericalCoordinates();


        float x = cartesian.x;
        float y = cartesian.y;
        float z = cartesian.z;

        // 1. 半径 (r) を計算
        sc.radius = cartesian.magnitude; // Mathf.Sqrt(x * x + y * y + z * z) と同じ

        if (sc.radius == 0)
        {
            // 原点にある場合、角度は定義不能
            sc.azimuthDegrees = 0f;
            sc.polarAngleDegrees = 0f;
            return sc;
        }

        // 2. 方位角 (θ) を計算（ラジアン）
        // Mathf.Atan2(y, x) は XZ平面ではなくXY平面で角度を計算するため、引数を (z, x) にする
        float azimuthRad = Mathf.Atan2(z, x);

        // 3. 天頂角 (φ) を計算（ラジアン）
        // Mathf.Clamp(y / sc.radius, -1.0f, 1.0f) で浮動小数点誤差を吸収
        float polarAngleRad = Mathf.Acos(Mathf.Clamp(y / sc.radius, -1.0f, 1.0f));

        // ラジアンを度に変換
        sc.azimuthDegrees = azimuthRad * Mathf.Rad2Deg;
        sc.polarAngleDegrees = polarAngleRad * Mathf.Rad2Deg;

        // 方位角を0～360度の範囲に正規化（オプション）
        //if (sc.azimuthDegrees < 0)
        //{
        //    sc.azimuthDegrees += 360f;
        //}

        return sc;
    }

    // (前の回答の CartesianToSphericalConverter クラス内に定義されたものと仮定)
    // public struct SphericalCoordinates {...}
    // public static SphericalCoordinates ConvertToSpherical(Vector3 cartesian) {...}

    public SphericalCoordinates GetObjectSphericalCoords(Transform objTransform, Vector3 center)
    {
        // オブジェクトのワールド座標を原点からの相対位置に変換
        Vector3 relativePosition = objTransform.position - center;
        // 球面座標に変換
        return CartesianToSphericalConverter.ConvertToSpherical(relativePosition);
    }
}