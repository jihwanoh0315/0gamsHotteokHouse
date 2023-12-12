using UnityEngine;

public class L_StartDialogue : Node
{
    DialogueManager m_theDM;
    DatabaseManager m_theDBM;
    int ID_;
    int m_start;
    int m_end;
    public L_StartDialogue( int ID_,
                                               DialogueManager theDM_, DatabaseManager theDBM_,
                                              int start_, int end_)
    {
        m_theDM= theDM_;
        m_theDBM = theDBM_;
        m_start= start_;
        m_end= end_;
    }

    public override NodeState Evaluate()
    {
        if (m_theDM != null && m_theDBM != null)
        {
            if (!m_theDM.isDialogue)
            {
                m_theDBM.GetDialogue(0, m_start, m_end);
                m_state = NodeState.SUCCESS;
                return m_state;
            }


        }
        // Running because not need go next
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
