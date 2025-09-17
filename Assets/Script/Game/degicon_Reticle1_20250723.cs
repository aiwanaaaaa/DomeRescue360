using System.Collections.Generic;
using UnityEngine;

public class Reticle1 : MonoBehaviour
{

    [SerializeField] GameObject sensor;
    private SensorFetvher data;
    [SerializeField] GameObject reticlePrefab;
    private Dictionary<string, GameObject> reticleMap = new();

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

                    if(x > 0 && x < 30)  x *= 6;
                    else if(x > 30 && x < 180)  x = 200;
                    else if (x > 330 && x < 360) x = (x - 360) * 6;
                    else if (x < 330 && x > 260) x = -190;

                    if(y > 50 && y < 180)  x = 100 * 2; 
                    if(y > 180 && y < 310)  x = -100 * 2; 
                    reticle.transform.localPosition = new Vector3(-x, y, 0); // World Space Canvasなら意外とこれが効きます

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
            GameObject reticle = Instantiate(reticlePrefab);
            reticleMap[id] = reticle;
        }
    }
}


