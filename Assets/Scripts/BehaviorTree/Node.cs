using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    RUNNING,
    SUCCESS,
    FAILURE
}

public class Node 
{
    protected NodeState m_state; 

    public Node m_parent;
    protected List<Node> m_children = new List<Node>();

    private Dictionary<string, object> m_dataContext = new Dictionary<string, object>();

    public Node()
    {
        m_parent = null;
    }

    public Node(List<Node> children_)
    {
        foreach (Node child in children_)
        {
            Attach(child);
        }
    }

    private void Attach(Node node_)
    {
        node_.m_parent = this;
        m_children.Add(node_);
    }

    public virtual NodeState Evaluate() => NodeState.FAILURE;

    public void SetData(string key_, object value_)
    {
        m_dataContext[key_] = value_;
    }

    public object GetData(string key_)
    {
        object value = null;
        if(m_dataContext.TryGetValue(key_, out value))
        {
            return value;
        }

        Node node = m_parent;
        while (node != null)
        {
            value = node.GetData(key_);
            if(value != null)
            {
                return value;
            }
            node = node.m_parent;
        }
        return null;
    }
}
