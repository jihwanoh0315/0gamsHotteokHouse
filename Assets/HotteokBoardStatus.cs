using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotteokBoardStatus : MonoBehaviour
{
    [SerializeField] List<Hotteok> hotteoks = new List<Hotteok>();
    [SerializeField] List<Hotteok> hotteoksOnRid = new List<Hotteok>();

    [SerializeField] public float oilTemprature = 1.0f;
    [SerializeField] public float toastAmount = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(oilTemprature < 0.4f)
        {
            toastAmount = 0.5f;
        }
        else if(oilTemprature < 0.9f)
        {
            toastAmount = 1.0f;
        }
    }
}
