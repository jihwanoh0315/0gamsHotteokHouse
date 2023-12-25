using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayManager;
using UnityEngine.XR;

public class HotteokContainers : MonoBehaviour
{
    [SerializeField] protected int m_price;
    [SerializeField] public int m_dough;
    [SerializeField] public int m_count;

    int containersMask;

    // Start is called before the first frame update
    public void Start()
    {
        m_price = 0;
        m_count = 0;
        containersMask = (1 << LayerMask.NameToLayer("Containers"));
    }

    public void Update()
    {
    }


    public virtual int GetPrice()
    {
        return m_price;
    }

    public bool CanMove()
    {
        return m_count > 0;
    }

    public bool IsMoving()
    {
        return true;
    }

    public virtual void RemoveHotteok()
    {

    }
    public virtual void ResetContainer() 
    {
    }

}
