using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Reticle_Move : MonoBehaviour
{

    [SerializeField] GameObject sensor;
    private SensorFetvher data;
    [SerializeField] GameObject reticlePrefab;
    [SerializeField] GameObject[] reticle;
    private Dictionary<string, GameObject> reticleMap = new();
    private RectTransform Recttransform;
    public float speed = 100.0f;

    private bool gamestart = false;

    void Start()
    {

    }

    void Update()
    {
        if (gamestart)
        {
            foreach (string id in data.deviceIdList)
            {
                if (data.sensorMap.ContainsKey(id))
                {
                    var sensor = data.sensorMap[id];
                    var reticle = reticleMap[id];
                    float x = sensor.alpha;
                    float y = sensor.beta;

                    Vector3 targetPosition;
                    if (x > 180)
                    {
                       targetPosition = new Vector3((360-x)*6, y*4, 0f);
                    }
                    else
                    {
                       targetPosition = new Vector3((-x)*6, y*4, 0f);
                    }
                   // Debug.Log(targetPosition);

                    Recttransform.localPosition = Vector3.MoveTowards(
                        Recttransform.localPosition,     // 現在地
                        targetPosition,         // 目標地
                        speed * Time.deltaTime  // 一度に移動する最大距離
                    );


                }
                else
                {
                    Debug.LogWarning($"Sensor data not found for ID: {id}");
                }

            }
        }

        //自身のタイミングで作動させる用
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gamestart = true;
        }
    }

    void OnEnable()
    {
        data = sensor.GetComponent<SensorFetvher>();
        data.OnDeviceIdsUpdated.AddListener(GenerateReticles);


    }

    void GenerateReticles(List<string> ids)
    {
        foreach (string id in ids)
        {
            reticleMap[id] = reticlePrefab;
        }
        for (int i = 0; i < data.deviceIdList.Count; i++)
        {
            reticle[i].SetActive(true);
            Recttransform = reticle[i].GetComponent<RectTransform>();
        }

    }
}


