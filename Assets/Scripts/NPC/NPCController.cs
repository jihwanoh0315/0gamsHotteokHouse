using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    public bool NPC_move;
    public string[] direction;
    public float speed;

}

public class NPCController : MonoBehaviour
{
    [SerializeField]
    public NPCMove npc;

    // Moving Limit
    public Vector2 m_moveLimit;
    public float m_moveSpeed;
    private float m_actualSpeed;
    private Vector3 m_movVec;

    // Timer
    public float m_moveTerm;
    public float m_moveTimer;
    private float m_currTimer;

    protected bool m_isMoving = false;

    public Animator m_animator;

    // Moving Order Queue
    public Queue<string> m_movingOrder;


    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_movingOrder = new Queue<string>();
        StartCoroutine(MoveCoroutine());
    }

    public void SetMove()
    {

    }

    public void SetNotMove()
    {
        m_movVec.Set(0.0f, 0.0f, m_movVec.z);
        m_animator.SetBool("isWalking", false);
    }

    public void Move(string dir_)
    {
        m_movingOrder.Enqueue(dir_);
        if (!m_isMoving)
        {
            m_isMoving = true;
            StartCoroutine(MoveCoroutineQueue());
        }
    }

    IEnumerator MoveCoroutineQueue()
    {
        while(m_movingOrder.Count != 0)
        {
            string dir = m_movingOrder.Dequeue();
            m_movingOrder.Enqueue(dir);

            // Set move dir
            m_movVec.Set(0.0f, 0.0f, m_movVec.z);
            switch (dir)
            {
                case "L":
                    m_movVec.x = -1.0f;
                    m_animator.SetBool("isWalking", true);
                    break;
                case "R":
                    m_movVec.x = 1.0f;
                    m_animator.SetBool("isWalking", true);
                    break;
                case "S":
                    m_movVec.x = 0.0f;
                    m_animator.SetBool("isWalking", false);
                    break; //stay
                default:
                    break;
            }
            m_animator.SetFloat("DirX", m_movVec.x);

            // Actual movement
            if (m_movVec.x != 0)
            {
                m_actualSpeed = m_moveSpeed;
                if (m_movVec.x < 0.0f) // L
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f);
                    if (transform.position.x < m_moveLimit.x)
                        m_actualSpeed = 0.0f;
                }
                else // R
                {
                    transform.localScale = new Vector3(1.0f, 1.0f);
                    if (transform.position.x > m_moveLimit.y)
                        m_actualSpeed = 0.0f;
                }
                while (m_currTimer < m_moveTimer)
                {
                    transform.Translate(m_movVec.x * m_actualSpeed * Time.deltaTime, 0, 0);
                    m_currTimer += Time.deltaTime;
                    yield return null;
                }
                m_currTimer = 0.0f;
                if (m_moveTerm > 0.0f)
                {
                    m_animator.SetBool("isWalking", false);

                    // delay the move
                    while (m_currTimer < m_moveTerm)
                    {
                        m_currTimer += Time.deltaTime;
                        yield return null;
                    }
                    m_currTimer = 0.0f;
                }
            }
            else
            {
                while (m_currTimer < 8.0f)
                {
                    m_currTimer += Time.deltaTime;
                    yield return null;
                }
                m_currTimer = 0.0f;
            }
        }
        m_animator.SetBool("isWalking", false);
        m_isMoving = false;
    }

    IEnumerator MoveCoroutine()
    {
        if (npc.direction.Length != 0)
        {
            for(int i = 0; i < npc.direction.Length; i++)
            {
                //yield return new WaitForSeconds(1.0f);

                yield return new WaitUntil(() => m_movingOrder.Count < 2);
                Move(npc.direction[i]);

                if(i == npc.direction.Length - 1)
                {
                    i = -1;
                }
            }

        }
        yield return null;
    }
}
   
