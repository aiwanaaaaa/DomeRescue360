using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SensorGet : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetSensorValues());
    }

    IEnumerator GetSensorValues()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/sensor/values");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("í êMÉGÉâÅ[: " + www.error);
        }
        else
        {
            string json = "{\"data\":" + www.downloadHandler.text + "}";
            SensorDataList dataList = JsonUtility.FromJson<SensorDataList>(json);

            foreach (var data in dataList.data)
            {
                Debug.Log($"ID: {data.id}, Éø: {data.alpha}, É¿: {data.beta}, É¡: {data.gamma}, ax: {data.accel_x}, ay: {data.accel_y}, az: {data.accel_z}");
            }
        }
    }

    [System.Serializable]
    public class SensorData
    {
        public int id;
        public float alpha, beta, gamma;
        public float accel_x, accel_y, accel_z;
    }

    [System.Serializable]
    public class SensorDataList
    {
        public List<SensorData> data;
    }
}