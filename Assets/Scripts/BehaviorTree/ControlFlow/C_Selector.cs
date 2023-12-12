using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Selector : Node
{
    public C_Selector() : base() { }
    public C_Selector(List<Node> children_) : base(children_) { }

    public override NodeState Evaluate()
    {
        foreach (Node node in m_children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    continue;
                case NodeState.SUCCESS:
                    m_state = NodeState.SUCCESS;
                    return m_state;
                case NodeState.RUNNING:
                    m_state = NodeState.RUNNING;
                    return m_state;
                default:
                    continue;
            }
        }
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
