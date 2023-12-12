using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveDemo : MonoBehaviour
{
    public float m_speed = 3.0f;
    public float m_runSpeed = 1.0f;
    float m_actualSpeed = 0.0f;

    public Vector2 m_moveLimit = new Vector2(10.0f, 10.0f);

    private Vector3 m_movVec;

    // Start is called before the first frame update
    void Start()
    {
        m_movVec = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_actualSpeed = m_speed + m_runSpeed;
            }
            else
            {
                m_actualSpeed = m_speed;
            }

            m_movVec.Set(Input.GetAxisRaw("Horizontal"), transform.position.y, transform.position.z);

            if (m_movVec.x != 0)
            {
                if (m_movVec.x < 0.0f) // L
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f);
                    if (transform.position.x < -m_moveLimit.x)
                        m_actualSpeed = 0.0f;
                }
                else // R
                {
                    transform.localScale = new Vector3(1.0f, 1.0f);
                    if (transform.position.x > m_moveLimit.y)
                        m_actualSpeed = 0.0f;
                }
                transform.Translate(m_movVec.x * m_actualSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
