using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class OP_Manager : MonoBehaviour
{
    [SerializeField] private GameObject qr_code;
    [SerializeField] private GameObject feed;
    [SerializeField] private float qr_time;
    [SerializeField] private float recture_time;
    private float time = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Debug.Log(time);

        if (time >= qr_time)
            qr_code.SetActive(false);

        if (time >= recture_time)
 
            feed.gameObject.transform.localScale += new Vector3(10,10,0);
    }
}
