using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class SensorFetvher : MonoBehaviour
{
    public List<string> deviceIdList = new List<string>();              //ユーザーIDを格納するリスト
    public Dictionary<string, SensorData> sensorMap = new();            // 各デバイスのセンサーデータ
    public UnityEvent<List<string>> OnDeviceIdsUpdated;

    // 外部から安全に最新の Quaternion を取得するためのプロパティ
    public Quaternion LatestRotation { get; private set; } = Quaternion.identity;
    void Start()
    {
        StartCoroutine(FetchDeviceIds());
    }
    IEnumerator FetchDeviceIds()
    {
        //URLに接続
        string url = $"http://localhost:3000/deviceIds";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        //URLに接続出来たか確認
        if (request.result == UnityWebRequest.Result.Success)
        {
            //接続出来たらユーザーID取得からリストに格納
            string json = request.downloadHandler.text;
            deviceIdList = JsonArrayParsor.Parse(json);
            OnDeviceIdsUpdated.Invoke(deviceIdList); // ← イベント発火！
            //Debug.Log($"取得された Device ID 数: {deviceIdList.Count}");
            foreach (string id in deviceIdList)
            {
                Debug.Log($" Device ID: {id}");
            }

            //IDを使ってその他の値を取得しに行く
            StartCoroutine(PollSensorData(deviceIdList));
        }
        //出来なかった場合エラーを出す
        else
        {
            Debug.LogError($"接続エラー: {request.error}");
        }
    }

    IEnumerator PollSensorData(List<string> deviceId)
    {
        //無限ループ
        while (true)
        {
            foreach (string id in deviceIdList)
            {
                //URLに接続
                UnityWebRequest req = UnityWebRequest.Get($"http://localhost:3000/sensor/latest/{id}");
                yield return req.SendWebRequest();

                //接続できるか確認
                if (req.result == UnityWebRequest.Result.Success)
                {
                    //接続出来たら値取得＋コンソールに出力
                    SensorData data = JsonUtility.FromJson<SensorData>(req.downloadHandler.text);
                    sensorMap[id] = data;

                    LatestRotation = Quaternion.Euler(data.alpha, data.beta, data.gamma);
                    //Debug.Log($"受信 - α:{data.alpha} β:{data.beta} γ:{data.gamma} x:{data.accel_x} y:{data.accel_y} z:{data.accel_z}");
                }
                else
                {
                    Debug.LogError($"通信エラー: {req.error}");
                }
            }

            //毎フレーム更新すると死ぬので200msごとに更新（ゲームの動きを見て調整必須）
            yield return new WaitForSeconds(0.05f); // ← 50msごとに更新
        }
    }
}