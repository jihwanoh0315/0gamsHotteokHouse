using UnityEngine;

public class L_ReadDialogues : Node
{
    //bool alreadyRead = false;
    public L_ReadDialogues()
    {
    }

    public override NodeState Evaluate()
    {


        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
