using System.Runtime.CompilerServices;
using UnityEngine;

public class SensorTest : MonoBehaviour
{
    [SerializeField] private SensorGet sensor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensor = GetComponent<SensorGet>();
    }

    // Update is called once per frame
    void Update()
    {
        int id = sensor.GetInstanceID();
    }
}
