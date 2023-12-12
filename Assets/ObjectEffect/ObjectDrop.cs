using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ObjectDrop : MonoBehaviour
{
    public float m_currY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_currY = transform.localPosition.y;
        //m_decreaseHeight = m_bounceHeight / m_dropCount;
        //m_currPos = transform.position;
        //m_currTime = 0.5f;
        //transform.position = new Vector3(m_currPos.x, m_currY + m_bounceHeight, m_currPos.z);

        RectTransform rectTransform = GetComponent<RectTransform>();
        Image img = GetComponent<Image>();

        //rectTransform.DOAnchorPosY(m_currY - 700, 1, true).SetEase(Ease.OutBounce);


        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(rectTransform.DOAnchorPosY(m_currY - 700, 1, true).SetEase(Ease.OutBounce));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    RectTransform rectTransform = GetComponent<RectTransform>();
        //    rectTransform.DOAnchorPosY(m_currY, 1, true).SetEase(Ease.OutBounce);
        //}
    }
}
