using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SensorFetvher : MonoBehaviour
{
    public List<string> deviceIdList = new List<string>();

    public SensorData data;

    void Start()
    {
        StartCoroutine(FetchDeviceIds());
    }
    IEnumerator FetchDeviceIds()
    {
        string url = $"http://localhost:3000/deviceIds";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            deviceIdList = JsonArrayParsor.Parse(json);
            Debug.Log($"取得された Device ID 数: {deviceIdList.Count}");
            foreach (string id in deviceIdList)
            {
                Debug.Log($" Device ID: {id}");
            }

            StartCoroutine(PollSensorData(deviceIdList));
        }
        else
        {
            Debug.LogError($"接続エラー: {request.error}");
        }
    }

    IEnumerator PollSensorData(List<string> deviceId)
    {
        while (true)
        {
            foreach (string id in deviceIdList)
            {
                UnityWebRequest req = UnityWebRequest.Get($"http://localhost:3000/sensor/latest/{id}");
                yield return req.SendWebRequest();

                if (req.result == UnityWebRequest.Result.Success)
                {
                    data = JsonUtility.FromJson<SensorData>(req.downloadHandler.text);
                    Debug.Log($"受信 - α:{data.alpha} β:{data.beta} γ:{data.gamma} x:{data.accel_x} y:{data.accel_y} z:{data.accel_z}");
                }
                else
                {
                    Debug.LogError($"通信エラー: {req.error}");
                }
            }

            yield return new WaitForSeconds(0.2f); // ← 200msごとに更新
        }
    }
}