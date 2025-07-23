using UnityEngine;

//[System.Serializable]
//public class SensorData
//{
//    public int id;
//    public string device_id;
//    public float alpha;
//    public float beta;
//    public float gamma;
//    public float accel_x;
//    public float accel_y;
//    public float accel_z;
//    public long timestamp;
//}

[System.Serializable]
public class SensorDataList
{
    public SensorData[] data;
}

[System.Serializable]
public class SensorData
{
    public float alpha;
    public float beta;
    public float gamma;
    public float accel_x;
    public float accel_y;
    public float accel_z;
    public long timestamp;
}

