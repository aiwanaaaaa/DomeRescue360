using UnityEngine;

public class RayCast : MonoBehaviour
{
    [SerializeField] GameObject sensor;

    RaycastHit hit;

    public GameObject[] reticle;
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        for (int i = 0; i < reticle.Length; i++)
        {
            Ray ray = cam.ScreenPointToRay(reticle[0].transform.position);
            
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

}
