using UnityEngine;

public class L_MoveCharacter : Node
{
    private GameObject m_chartoMove; //!< to block repeating

    bool m_toLeft = false;
    float m_movDir = 1.0f;
    float m_movSpeed = 1.0f;
    float m_targetPos = 0.0f;
    public L_MoveCharacter(GameObject charToMove_, bool toLeft_,float targetPos_, float movSpeed_)
    {
        m_chartoMove = charToMove_;
        
        if(toLeft_)
        {
            m_toLeft=true;
            m_movDir = -1.0f;
        }
        m_targetPos = targetPos_;
        m_movSpeed = m_movDir * movSpeed_;
    }

    public override NodeState Evaluate()
    {
        //m_chartoMove.m_currNode = "L_MoveCharacter";

        Vector3 currPos = m_chartoMove.transform.position;

        if (m_toLeft)
        {
            if(currPos.x < m_targetPos)
            {
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }
        else
        {
            if (currPos.x > m_targetPos)
            {
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }

        m_chartoMove.transform.position = new Vector3(currPos.x + m_movSpeed, currPos.y, currPos.z);

        // Running because not need go next
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
