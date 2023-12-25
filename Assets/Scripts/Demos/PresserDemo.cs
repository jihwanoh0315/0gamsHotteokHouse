using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresserDemo : MonoBehaviour
{
    public GameObject m_presserHand;
    public GameObject m_HT_toPress;
    public GameObject m_HT_toChangeColor;

    public Sprite m_roastedDough;
    public Sprite m_pressedDough;


    private float m_currTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_currTime += Time.deltaTime;
        if (m_currTime >= 3.0f)
            m_HT_toChangeColor.GetComponent<SpriteRenderer>().sprite = m_roastedDough;

        if(Input.GetMouseButtonDown(0))
        {
            m_HT_toPress.GetComponent<SpriteRenderer>().sprite = m_pressedDough;
            m_presserHand.transform.localScale = new(0.75f, 0.75f);
            StartCoroutine(MovePresser());

        }
    }

    IEnumerator MovePresser()
    {
        while (m_presserHand.transform.localScale.x < 1.0f)
        {
            m_presserHand.transform.localScale
                = new(m_presserHand.transform.localScale.x + Time.deltaTime, m_presserHand.transform.localScale.y + Time.deltaTime, 0.0f);;
            yield return null;
        }
    }
}
