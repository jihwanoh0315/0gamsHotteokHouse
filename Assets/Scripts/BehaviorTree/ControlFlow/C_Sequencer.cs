using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_Sequencer : Node
{
    public C_Sequencer() : base() { }
    public C_Sequencer(List<Node> children_) : base(children_) { }
    public override NodeState Evaluate()
    {
        bool anyChildIsRunning = false;
        foreach (Node node in m_children)
        {
            switch (node.Evaluate())
            {
                case NodeState.FAILURE:
                    m_state = NodeState.FAILURE;
                    return m_state;
                case NodeState.SUCCESS:
                    continue;
                case NodeState.RUNNING:
                    anyChildIsRunning = true;
                    continue;
                default:
                    m_state = NodeState.SUCCESS;
                    return m_state;
            }
        }
        m_state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return m_state;
    }
}
