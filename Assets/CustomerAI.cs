using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public class CustomerStatus
{
    public SpriteRenderer Emoji;
    public SpriteRenderer OrderCount;

    public int orderCount;
}

public class CustomerAI : MonoBehaviour
{

    public CustomerStatus m_status;
    [SerializeField]
    private EmoteManager m_theEM;

    // Start is called before the first frame update
    void Start()
    {
        m_theEM = FindObjectOfType<EmoteManager>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    void SetOrderCount()
    {
        int currCount = Random.Range(1, 6);
        m_status.orderCount = currCount;
        // ID = 12000
        m_status.OrderCount.GetComponent<SpriteRenderer>().sprite = m_theEM.emoteDic[12000 + currCount - 1];
    }
}
