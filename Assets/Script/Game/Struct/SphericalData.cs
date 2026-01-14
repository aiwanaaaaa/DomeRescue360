using UnityEngine;

// [System.Serializable] をつけると、Unityのインスペクター上で中身が見えるようになります
[System.Serializable]
public struct SphericalCoordinate
{
    public float radius;    // 距離
    public float azimuth;   // 方位角
    public float elevation; // 仰角

    // コンストラクタ（初期化用の関数）を作っておくと便利
    public SphericalCoordinate(float r, float a, float e)
    {
        radius = r;
        azimuth = a;
        elevation = e;
    }
}