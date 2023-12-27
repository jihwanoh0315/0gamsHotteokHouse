using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<CustomerAI> m_customers = new List<CustomerAI>();
    public List<CustomerAI> m_liningCustomers = new List<CustomerAI>();
    public List<CustomerAI> m_shoppingCustomers = new List<CustomerAI>();

    public GameObject[] customerPrefab;
    LevelSetting m_levelSetting;

    public CustomerAI m_customer_Ordering;

    // Curr Stage Initial values - from lvSetting
    int round_maxHotteok; // how much hotteok can sell
    int anotherDough = 0;

    // Customer Initial values
    public int customer_maxHotteok; // customer buy count (x1~x6 max)
    public int customer_maxVisitor; // customer character variation(~5)

    int expensiveHotteokRatio = 40; // 100 is max. ratio of expensive hotteok
    int buySideRatio = 20; // 100 is max. ratio of buying Side ratio

    float newCustomerCommingSecond = 10.0f; // initial speed
    float customerCanCommingEarly = 10.0f; // +- second for customer comming

    List<int> activeSides = new List<int>();
    List<int> soldSides = new List<int>();


    // Values in game
    public float SpawningTime = 15.0f;
    public float currTime = 14;

    bool isStoreOpened = true;

    int prevCharacter = 0;
    int vision = 2; // Possible to check hotteok of person on line

    // Start is called before the first frame update
    void Awake()
    {
        m_levelSetting = FindAnyObjectByType<LevelSetting>();
        anotherDough = m_levelSetting.activeFoods[1] ? 2 : m_levelSetting.activeFoods[0] ? 1 : 0;
        if (m_levelSetting.m_gameData.currDate < 2)
        {
            customer_maxHotteok = 1;
            customer_maxVisitor = 2;
        }
        else if (m_levelSetting.m_gameData.currDate < 5)
        {
            customer_maxHotteok = 4;
            customer_maxVisitor = 3;
        }
        else if (m_levelSetting.m_gameData.currDate < 7)
        {
            customer_maxHotteok = 4;
            customer_maxVisitor = 4;
        }
        else
        {
            customer_maxHotteok = 6;
            customer_maxVisitor = 5;
        }

        newCustomerCommingSecond = 5.0f;
        customerCanCommingEarly = 1.0f;

        if (m_levelSetting.activeFoods[2])
        {
            activeSides.Add(0);
            soldSides.Add(0);
        }
        if (m_levelSetting.activeFoods[3])
        {
            activeSides.Add(1);
            soldSides.Add(0);
        }
        if (m_levelSetting.activeFoods[4])
        {
            activeSides.Add(2);
            soldSides.Add(0);
        }

        if (m_levelSetting.activePet == 7) //inumaki
        {
            vision = 6;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_liningCustomers.Count; i++)
        {
            if (i < vision && m_liningCustomers[i].IsNotBusy())
            {
                m_liningCustomers[i].m_status.EmoteBox.gameObject.SetActive(true);
            }
        }
        // 시간되면 손님 소환
        if (isStoreOpened) // 오픈중이면
        {
            SpawnCustomer();
        }

        if (m_customer_Ordering == null)
        {
            if (m_liningCustomers.Count > 0)
            {
                SendFirstCustomer();
            }
        }

        List<CustomerAI> copyList = new List<CustomerAI>(m_shoppingCustomers);

        // 주문하는 손님들 확인해서 주문 끝나면 없애기
        foreach (CustomerAI customer in copyList)
        {
            if (customer.m_shoppingFinishied)
            {
                m_shoppingCustomers.Remove(customer);
                m_customers.Remove(customer);
            }
        }
    }


    void SpawnCustomer()
    {
        if(m_liningCustomers.Count < 5)
        {
            if (currTime > SpawningTime)
            {

                int toSpawn = 0;
                if(Random.Range(0,3) > 0)
                {
                    toSpawn = Random.Range(0, 2);
                }
                else
                {
                    while (prevCharacter == toSpawn)
                    {
                        toSpawn = Random.Range(0, customer_maxVisitor);
                    }
                    
                    prevCharacter = toSpawn;
                }


                // Spawn the object
                CustomerAI currCustomer = Instantiate(customerPrefab[toSpawn], new Vector3(-15.0f, 3.5f), Quaternion.identity).GetComponent<CustomerAI>();
                if (anotherDough != 0)
                {
                    currCustomer.SetDough(anotherDough, expensiveHotteokRatio);
                }
                currCustomer.SetOrderCount(customer_maxHotteok);
                SetSideMenu(currCustomer);
                currCustomer.GoTo( -0.2f - m_liningCustomers.Count * 2.0f);
                m_customers.Add(currCustomer);
                m_liningCustomers.Add(currCustomer);


                // Set Customer Spawning Time
                SpawningTime = newCustomerCommingSecond + Random.Range(0, customerCanCommingEarly);
                currTime = 0.0f;
            }
        currTime += Time.deltaTime;
        }

    }

    public void CloseTheStore()
    {
        isStoreOpened = false;
    }

    public int GetLeftCustomerNum()
    {
        return m_customers.Count;
    }

    void SellSide(CustomerAI customerAI_, int side_)
    {
        customerAI_.SetSideInTex(side_, activeSides[side_ - 1]);
        ++soldSides[side_ - 1];
    }

    void SetSideMenu(CustomerAI customerAI_)
    {
        if(activeSides.Count > 0)
        {
            if(activeSides.Count == 1)
            {
                int ranVal = Random.Range(0, buySideRatio);
                if (ranVal < buySideRatio)
                {
                    SellSide(customerAI_, 1);
                }
            }
            else
            {
                int sidePlace = Random.Range(1, 3);

                if(sidePlace == 1) // check 1 first
                {
                    int ranVal = Random.Range(0, buySideRatio);

                    if (ranVal < buySideRatio)
                    {
                        //buy the side
                        SellSide(customerAI_, 1);
                    }
                    else
                    {
                        ranVal = Random.Range(0, buySideRatio);
                        if (ranVal < buySideRatio)
                        {
                            //buy the side
                            SellSide(customerAI_, 2);
                        }
                    }
                }
                else // check 2 first
                {
                    int ranVal = Random.Range(0, buySideRatio);

                    if (ranVal < buySideRatio)
                    {
                        //buy the side
                        SellSide(customerAI_, 2);
                    }
                    else
                    {
                        ranVal = Random.Range(0, buySideRatio);
                        if (ranVal < buySideRatio)
                        {
                            //buy the side
                            SellSide(customerAI_, 1);
                        }
                    }
                }
            }
        }
    }

    void SendFirstCustomer()
    {
        CustomerAI customerAI = m_liningCustomers[0];
        if(customerAI.IsNotBusy())
        {
            //Debug.Log("SendFirstCustomer");
            m_customer_Ordering = customerAI;
            m_customer_Ordering.GoOrdering();
            m_liningCustomers.RemoveAt(0);

            int lineLength = m_liningCustomers.Count;
            for (int i = 0; i < lineLength; ++i)
            {
                m_liningCustomers[i].GoTo(-0.2f - 2.0f * i);
                if (i < vision)
                {
                    if (!m_liningCustomers[i].IsMoving())
                    {
                        m_liningCustomers[i].m_status.EmoteBox.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public bool GiveHotteokToCustomer(HotteokContainers containers_)
    {
        if(!m_customer_Ordering || !m_customer_Ordering.IsCanOrder())
            return false;

        bool result = m_customer_Ordering.GetHotteok(containers_);

       if(m_customer_Ordering.m_getAllHotteok)
       {
            m_shoppingCustomers.Add(m_customer_Ordering);
            m_customer_Ordering = null;
       }

        return result;
    }
}
