using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestMove
{
    public string name;
    public string direction;
}

public class NPCTester : MonoBehaviour
{
    [SerializeField]
    public TestMove[] m_move;

    private OrderManager m_theOrder;

    public string m_direction;

    // Start is called before the first frame update
    void Start()
    {
        m_theOrder = FindObjectOfType<OrderManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            m_theOrder.PreLoadCharacters();
            //for(int i = 0; i < m_move.Length; i++)
            //{
            //    m_theOrder.Move(m_move[i].name, m_move[i].direction);
            //}

            m_theOrder.Turn("Hou_NPC2", m_direction);
        }
    }
}
