using UnityEngine;

public class SphericalTarget : MonoBehaviour
{
    // 現在の球面座標を保持
    public SphericalCoordinate currentCoords;

    // プレイヤー（中心）からの相対位置を球面座標に変換して更新
    public void UpdateCoordinate(Vector3 centerPosition)
    {
        Vector3 relativePos = transform.position - centerPosition;
        float r = relativePos.magnitude;

        if (r > 0)
        {
            // CoordinateManagerと同じ計算式を使用
            float azimuth = Mathf.Atan2(relativePos.x, relativePos.z) * Mathf.Rad2Deg;
            float elevation = Mathf.Asin(relativePos.y / r) * Mathf.Rad2Deg;

            currentCoords = new SphericalCoordinate(r, azimuth, elevation);
        }
    }
}