using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPullOut : MonoBehaviour
{
    public enum Dir
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }


    public float m_pullSpeed = 5.0f;

    public Dir m_dir = Dir.LEFT;
    public bool m_isActive = false;


    private Vector3 m_currPos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isActive)
        {
            m_currPos = transform.position;
            float actualMove = Time.deltaTime * m_pullSpeed;

            switch (m_dir)
            {
                case Dir.LEFT:
                    transform.position = new Vector3(m_currPos.x - actualMove, m_currPos.y, m_currPos.z);
                    break;
                case Dir.RIGHT:
                    transform.position = new Vector3(m_currPos.x + actualMove, m_currPos.y, m_currPos.z);
                    break;
                case Dir.UP:
                    transform.position = new Vector3(m_currPos.x, m_currPos.y + actualMove, m_currPos.z);
                    break;
                case Dir.DOWN:
                    transform.position = new Vector3(m_currPos.x, m_currPos.y - actualMove, m_currPos.z);
                    break;
                default:
                    break;
            }
        }
    }
}
