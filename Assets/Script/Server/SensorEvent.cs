using UnityEngine;
using System.Collections.Generic;

public class SensorEvent : MonoBehaviour
{
    void OnEnable()
    {
        //SensorGet.OnSensorDataReceived += HandleSensorData;
    }

    void OnDisable()
    {
       // SensorFetcher.OnSensorDataReceived -= HandleSensorData;
    }

    //void HandleSensorData(List<SensorFetcher.SensorData> dataList)
    //{
    //    var latest = dataList[0];
    //    Debug.Log($"[SensorEvent] ç≈êVÇÃâ¡ë¨ìx: {latest.accel_x}, {latest.accel_y}, {latest.accel_z}");
    //}
}