using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughFoldDemo : MonoBehaviour
{
    public GameObject m_foldingHand;
    public GameObject m_dough;

    public GameObject[] m_directionMark;
    public Sprite[] m_directionDoughs;

    public float m_moveSpeed = 2.0f;
    public float m_scaleSpeed = 2.0f;
    public float m_bigScale = 1.5f;

    Vector2 minMax;
    Vector2 centerPos;
    Vector2 TargetPos;
    public int m_currIndex = -1;
    float myZ = -3.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_currIndex = -1;
        minMax = new Vector2(1.0f, 1.2f);
        centerPos = new Vector2(0.0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            m_currIndex++;
            m_dough.GetComponent<SpriteRenderer>().sprite = m_directionDoughs[m_currIndex];
           switch (m_currIndex)
            {
                case 0:
                    m_foldingHand.transform.localPosition = new Vector3(minMax.x, centerPos.y, myZ);
                    TargetPos = new Vector2(-minMax.x, centerPos.y);
                    break;
                case 1:
                    m_foldingHand.transform.localPosition = new Vector3(centerPos.x, -minMax.y, myZ);
                    TargetPos = new Vector2(centerPos.x, minMax.y);
                    break;
                case 2:
                    m_foldingHand.transform.localPosition = new Vector3(-minMax.x, centerPos.y, myZ);
                    TargetPos = new Vector2(minMax.x, centerPos.y);
                    break;
                case 3:
                    m_foldingHand.transform.localPosition = new Vector3(centerPos.x, minMax.y, myZ);
                    TargetPos = new Vector2(centerPos.x, -minMax.y);
                    break;
                default:
                    foreach(GameObject gO in m_directionMark)
                    {
                        Color tmp = gO.GetComponent<SpriteRenderer>().color;
                        tmp.a = 1.0f;
                        gO.GetComponent<SpriteRenderer>().color = tmp;
                        gO.SetActive(true);
                        gO.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
                    }
                    m_currIndex = -1;
                    break;

            }
            if(m_currIndex < 4)
                StartCoroutine(DirectionArrowExplode());
            StartCoroutine(MoveHand());
            
        }
    }

    IEnumerator MoveHand()
    {
        switch (m_currIndex)
        {
            case 0:
                while(TargetPos.x < m_foldingHand.transform.localPosition.x)
                {
                    m_foldingHand.transform.Translate(-Time.deltaTime * m_moveSpeed, 0.0f, 0.0f);
                    yield return null;
                }
                break;
            case 1:
                while (TargetPos.y > m_foldingHand.transform.localPosition.y)
                {
                    m_foldingHand.transform.Translate(0.0f, Time.deltaTime * m_moveSpeed, 0.0f);
                    yield return null;
                }
                break;
            case 2:
                while (TargetPos.x > m_foldingHand.transform.localPosition.x)
                {
                    m_foldingHand.transform.Translate(Time.deltaTime * m_moveSpeed, 0.0f, 0.0f);
                    yield return null;
                }

                break;
            case 3:
                while (TargetPos.y < m_foldingHand.transform.localPosition.y)
                {
                    m_foldingHand.transform.Translate(0.0f, -Time.deltaTime * m_moveSpeed, 0.0f);
                    yield return null;
                }
                break;
            default:
                break;
        }

    }
    IEnumerator DirectionArrowExplode()
    {
        while (m_directionMark[m_currIndex].transform.localScale.x < m_bigScale)
        {
            float newScale = m_directionMark[m_currIndex].transform.localScale.x + Time.deltaTime * m_scaleSpeed;
            m_directionMark[m_currIndex].transform.localScale
                = new Vector3(newScale, newScale, m_directionMark[m_currIndex].transform.localScale.z);
            Color tmp = m_directionMark[m_currIndex].GetComponent<SpriteRenderer>().color;
            tmp.a -= Time.deltaTime * m_bigScale;
            m_directionMark[m_currIndex].GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }
        m_directionMark[m_currIndex].SetActive(false);
    }


}
