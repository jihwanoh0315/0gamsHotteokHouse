using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPullOut;

public class OrderManager : MonoBehaviour
{
    private PlayerController playerController; // block the key input controller
    private List<NPCController> NPCs;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void PreLoadCharacters()
    {
        NPCs = ToList();
    }

    public List<NPCController> ToList()
    {
        List<NPCController> tempList = new List<NPCController>();
        NPCController[] temp = FindObjectsOfType<NPCController>();

        for(int i = 0; i < temp.Length; i++)
        {
            tempList.Add(temp[i]);
        }
        return tempList;
    }

    public void Move(string name_, string dir_)
    {
        for (int i = 0; i < NPCs.Count; i++)
        {
            if(name_ == NPCs[i].name)
            {
                NPCs[i].Move(dir_);
            }
        }
    }

    public void Turn(string name_, string dir_)
    {
        for (int i = 0; i < NPCs.Count; i++)
        {
            if (name_ == NPCs[i].name)
            {
                NPCs[i].m_animator.SetFloat("DirX", 0.0f);

                if(dir_ == "R")
                {
                    NPCs[i].transform.localScale = new Vector3(1.0f, 1.0f);
                }
                else if   (dir_ == "L")
                {
                    NPCs[i].transform.localScale = new Vector3(-1.0f, 1.0f);
                }
            }
        }
    }

    public void SetTransparent(string name_, bool transparent_)
    {
        for (int i = 0; i < NPCs.Count; i++)
        {
            if (name_ == NPCs[i].name)
            {
                NPCs[i].gameObject.SetActive(transparent_);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
