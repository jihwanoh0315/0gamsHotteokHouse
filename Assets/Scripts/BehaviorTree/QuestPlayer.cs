using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

public class QuestPlayer : BHTree
{

    public int m_ID;
    [SerializeField] GameObject m_leftChar;
    [SerializeField] GameObject m_rightChar;

    DialogueManager theDM;
    DatabaseManager theDBM;


    protected void Start()
    {
        m_owner = gameObject;
        m_root = SetupTree();
        theDM = FindObjectOfType<DialogueManager>();
        theDBM = FindObjectOfType<DatabaseManager>();
    }

    protected override Node SetupTree()
    {
        Node root = new C_Selector(new List<Node>
        {
            new C_Sequencer(new List<Node>
            {
                new L_StartDialogue(m_ID, theDM, theDBM, 1, 5),
                new L_MoveCharacter(m_rightChar, true, 0.0f, 1.0f)
            }),
        }); ;
        return root;
    }

}
