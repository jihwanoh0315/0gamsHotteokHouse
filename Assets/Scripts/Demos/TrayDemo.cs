using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrayDemo : MonoBehaviour
{
    public GameObject m_trayHand;
    public GameObject m_TongsHand;

    public GameObject[] m_HT_Inactive;
    public GameObject[] m_HT_Active;

    private int m_currCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_TongsHand.transform.Rotate(0.0f, 0.0f, 20.0f);
            m_trayHand.transform.Translate(0.0f, -0.5f, 0.0f);
            StartCoroutine(MoveHand());
            StartCoroutine(MoveTongs());

            if(m_currCount < 3)
            {
                m_HT_Inactive[m_currCount].SetActive(true);
                m_HT_Active[m_currCount].SetActive(false);
                m_currCount++;
            }

        }
    }

    //IEnumerator 
    IEnumerator MoveHand()
    {
        while(m_trayHand.transform.position.y < -4.0f)
        {
            m_trayHand.transform.Translate(0.0f, Time.deltaTime, 0.0f);
            yield return null;
        }


    }
    IEnumerator MoveTongs()
    {
        while(m_TongsHand.transform.rotation.z > 0)
        {
            m_TongsHand.transform.Rotate(0.0f, 0.0f, -Time.deltaTime * 100.0f, Space.World);
            yield return null;
        }
    }

}
