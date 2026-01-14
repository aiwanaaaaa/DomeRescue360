// 判定を行うメインクラス
using UnityEngine;
using static CartesianToSphericalConverter;

public class EquirectangularHitChecker : MonoBehaviour
{


    // === Inspector設定項目 ===

    [Header("Dependencies")]
    // サーバー通信クラスへの参照
    [SerializeField] GameObject sensor;
    private SensorFetvher data;

    //サーバーから取得した角度情報
    float alpha;
    float beta;
    float gamma;

    // パノラマの中心となる点（プレイヤーの位置）
    public Transform centerPointTransform;

    [Header("Hit Settings")]
    // レティクル（照準）の許容角度（度）。この角度差以内ならヒットと見なす
    [Tooltip("AzimuthとPolar Angleの角度差がこの値以下であればヒットと判定")]
    public float hitToleranceDegrees = 3.0f;

    // 当たり判定の対象となるオブジェクトのタグ
    public string targetTag = "HittableObject";

    // === 内部変数 ===
    private Vector3 centerPoint = Vector3.zero;
    void Start()
    {
        data = sensor.GetComponent<SensorFetvher>();
        foreach (string id in data.deviceIdList)
        {
            if (data.sensorMap.ContainsKey(id))
            {
                var sensor = data.sensorMap[id];
                alpha = sensor.alpha;
                beta = sensor.beta;
                gamma = sensor.gamma;
            }
        }

    }


    void Update()
    {
        // 中心点座標の更新 (Transformが設定されていればその位置を使用)
        centerPoint = centerPointTransform != null ? centerPointTransform.position : Vector3.zero;

        // 1. スマホの向き（カメラの向き）を取得
        Quaternion latestRot = data.LatestRotation;
        Vector3 forwardVector = latestRot * Vector3.forward;

        // 2. スマホの向きを球面座標の角度に変換
        SphericalCoordinates cameraSpherical = GetCameraSphericalCoords(forwardVector);

        // 3. 当たり判定の実行
        CheckForHits(cameraSpherical);
    }
    // ... GetCameraSphericalCoords メソッド, GetObjectSphericalCoords メソッド, SphericalCoordinates Struct, ConvertToSpherical メソッドをここに追加 ...

    private void CheckForHits(SphericalCoordinates cameraSpherical)
    {
        // タグに基づいて判定対象のオブジェクトを検索
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("TargetTag");

        foreach (GameObject obj in targetObjects)
        {
            // オブジェクトの球面座標の角度を取得
            SphericalCoordinates objectSpherical = GetObjectSphericalCoords(obj.transform, centerPoint);

            // 角度の差分を計算し、当たり判定を行う
            if (IsHitting(cameraSpherical, objectSpherical, hitToleranceDegrees))
            {
                // ★★★ 重なっていた場合の処理をここに記述 ★★★
                ExecuteHitAction(obj);
                Debug.Log("重なったやで");
            }
            // 必要であれば、重なっていない場合の処理を else if などで記述
        }
    }

    /// <summary>
    /// 当たり判定が成功した際の具体的な処理
    /// </summary>
    private void ExecuteHitAction(GameObject hitObject)
    {
        Debug.Log($"レティクルとオブジェクトが重なりました: {hitObject.name}");

        // 例: オブジェクトの色を変える
        Renderer renderer = hitObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 例として、赤色に変化させる
            renderer.material.color = Color.red;
        }

        // 例: 特定のコンポーネントのメソッドを呼び出す
        // hitObject.GetComponent<InteractiveObject>().Interact();
    }

    /// <summary>
    /// 向きベクトルから球面座標の角度を計算します (カメラ/レティクル用)
    /// </summary>
    private SphericalCoordinates GetCameraSphericalCoords(Vector3 forward)
    {
        SphericalCoordinates sc = new SphericalCoordinates();

        // ベクトル長は1なので、radiusは1
        sc.radius = 1f;

        // 1. 方位角 (θ) : Atan2(z, x)
        float azimuthRad = Mathf.Atan2(forward.z, forward.x);
        sc.azimuthDegrees = azimuthRad * Mathf.Rad2Deg;

        // 2. 天頂角 (φ) : Acos(y / r) = Acos(y)
        float polarAngleRad = Mathf.Acos(forward.y);
        sc.polarAngleDegrees = polarAngleRad * Mathf.Rad2Deg;

        // 0～360度に正規化
        if (sc.azimuthDegrees < 0)
        {
            sc.azimuthDegrees += 360f;
        }

        return sc;
    }

    /// <summary>
    /// オブジェクトのワールド位置から球面座標の角度を計算します
    /// </summary>
    private SphericalCoordinates GetObjectSphericalCoords(Transform objTransform, Vector3 center)
    {
        Vector3 relativePosition = objTransform.position - center;
        return ConvertToSpherical(relativePosition);
    }

    /// <summary>
    /// デカルト座標 (x, y, z) を球面座標 (r, θ, φ) に変換します。
    /// </summary>
    private SphericalCoordinates ConvertToSpherical(Vector3 cartesian)
    {
        SphericalCoordinates sc = new SphericalCoordinates();

        float x = cartesian.x;
        float y = cartesian.y;
        float z = cartesian.z;

        sc.radius = cartesian.magnitude;

        if (sc.radius == 0) return sc;

        float azimuthRad = Mathf.Atan2(z, x);
        float polarAngleRad = Mathf.Acos(Mathf.Clamp(y / sc.radius, -1.0f, 1.0f));

        sc.azimuthDegrees = azimuthRad * Mathf.Rad2Deg;
        sc.polarAngleDegrees = polarAngleRad * Mathf.Rad2Deg;

        // 0～360度に正規化
        if (sc.azimuthDegrees < 0)
        {
            sc.azimuthDegrees += 360f;
        }

        return sc;
    }

    /// <summary>
    /// 2つの球面座標の角度が許容範囲内にあるか判定します。
    /// </summary>
    private bool IsHitting(SphericalCoordinates camera, SphericalCoordinates target, float tolerance)
    {
        // 1. 方位角 (Azimuth/θ) の差分
        float azimuthDiff = Mathf.Abs(camera.azimuthDegrees - target.azimuthDegrees);

        // 角度差を最小化（180度を超える差分を補正）
        if (azimuthDiff > 180f)
        {
            azimuthDiff = 360f - azimuthDiff;
        }

        // 2. 天頂角 (Polar Angle/φ) の差分
        float polarDiff = Mathf.Abs(camera.polarAngleDegrees - target.polarAngleDegrees);

        // 3. 総合判定: 両方の角度差が許容範囲内であれば重なっている
        return (azimuthDiff <= tolerance) && (polarDiff <= tolerance);
    }
}