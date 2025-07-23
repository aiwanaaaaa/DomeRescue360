using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class SensorReceiver : MonoBehaviour
{
    private string url = "http://localhost:3000/sensor/values";

    [SerializeField] TextMeshProUGUI ID;
    [SerializeField] TextMeshProUGUI Alpha;
    [SerializeField] TextMeshProUGUI Beta;
    [SerializeField] TextMeshProUGUI Gamma;
    [SerializeField] TextMeshProUGUI Accel_X;
    [SerializeField] TextMeshProUGUI Accel_Y;
    [SerializeField] TextMeshProUGUI Accel_Z;

    IEnumerator GetSensorData()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("通信エラー: " + request.error);
        }
        else
        {
            string json = "{\"data\":" + request.downloadHandler.text + "}";
            Debug.Log("受信JSON: " + request.downloadHandler.text);
            SensorDataList list = JsonUtility.FromJson<SensorDataList>(json);

            foreach (var data in list.data)
            {
                Debug.Log($"受信 - α:{data.alpha} β:{data.beta} γ:{data.gamma} x:{data.accel_x} y:{data.accel_y} z:{data.accel_z}");


                Alpha.text = "ID:" + data.alpha;
                Beta.text = "ID:" + data.beta;
                Gamma.text = "ID:" + data.gamma;
                Accel_X.text = "ID:" + data.accel_x;
                Accel_Y.text = "ID:" + data.accel_y;
                Accel_Z.text = "ID:" + data.accel_z;
            }
        }
    }

    private void Update()
    {
        StartCoroutine(GetSensorData());
    }
}